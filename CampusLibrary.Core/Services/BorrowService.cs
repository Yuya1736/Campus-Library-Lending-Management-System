using CampusLibrary.Core.Data;
using CampusLibrary.Core.Models;
using CampusLibrary.Core.Repositories;
using Dapper;

namespace CampusLibrary.Core.Services;

// 借阅核心服务：
// 负责借书、还书、超期状态刷新，以及与库存/借阅记录的事务一致性。
public class BorrowService
{
    private readonly DbConnectionFactory _factory;
    private readonly BookRepository _books;
    private readonly BorrowRepository _borrows;
    private readonly LogRepository _logs;

    public BorrowService(DbConnectionFactory factory, BookRepository books, BorrowRepository borrows, LogRepository logs)
    {
        _factory = factory;
        _books = books;
        _borrows = borrows;
        _logs = logs;
    }

    public List<BorrowRecord> GetBorrowRecords() => _borrows.GetAll();

    public OperationResult BorrowBook(string operatorUsername, string studentId, string isbn, int borrowDays = 30)
    {
        // 先清洗输入，避免仅包含空格的参数。
        studentId = studentId.Trim();
        isbn = isbn.Trim();

        if (string.IsNullOrWhiteSpace(studentId))
        {
            return OperationResult.Fail("请输入学号");
        }

        if (string.IsNullOrWhiteSpace(isbn))
        {
            return OperationResult.Fail("请输入ISBN");
        }

        var book = _books.GetByIsbn(isbn);
        if (book is null)
        {
            return OperationResult.Fail("图书不存在");
        }

        if (book.StockQty <= 0)
        {
            return OperationResult.Fail("库存不足");
        }

        // 借书属于多表写入：借阅记录 + 图书库存 + 学生借阅次数，必须使用事务。
        using var conn = _factory.CreateOpenConnection();
        using var tx = conn.BeginTransaction();

        const string ensureStudentSql = @"
INSERT INTO student(student_id, name, class_name, contact, total_borrow_count)
SELECT @studentId, @name, @className, @contact, 0
WHERE NOT EXISTS (
    SELECT 1 FROM student WHERE student_id = @studentId
)";
        // 若学生不存在则自动创建“占位学生档案”，避免外键失败。
        conn.Execute(
            ensureStudentSql,
            new
            {
                studentId,
                name = $"学生-{studentId}",
                className = "未分班",
                contact = "-"
            },
            tx);

        var borrowDate = DateTime.Now;
        var dueDate = borrowDate.AddDays(borrowDays);

        const string insertRecord = @"
INSERT INTO borrow_record(isbn, student_id, borrow_date, due_date, status, overdue_days)
VALUES(@isbn, @studentId, @borrowDate, @dueDate, '借出中', 0)";
        conn.Execute(insertRecord, new { isbn, studentId, borrowDate, dueDate }, tx);

        // 借出后同步扣减库存并累计借阅次数。
        conn.Execute("UPDATE book SET stock_qty = stock_qty - 1, borrow_count = borrow_count + 1 WHERE isbn = @isbn", new { isbn }, tx);
        conn.Execute("UPDATE student SET total_borrow_count = total_borrow_count + 1 WHERE student_id = @studentId", new { studentId }, tx);

        tx.Commit();
        _logs.Add(operatorUsername, "借阅", $"学号:{studentId}, ISBN:{isbn}");
        return OperationResult.Ok("借书成功");
    }

    public OperationResult ReturnBook(string operatorUsername, string studentId, string isbn)
    {
        using var conn = _factory.CreateOpenConnection();

        const string selectSql = @"
SELECT record_id AS RecordId, isbn AS Isbn, student_id AS StudentId,
       borrow_date AS BorrowDate, due_date AS DueDate, return_date AS ReturnDate,
       status AS Status, overdue_days AS OverdueDays
FROM borrow_record
WHERE student_id = @studentId AND isbn = @isbn AND status = '借出中'
ORDER BY record_id DESC
LIMIT 1";

        // 按“学号 + ISBN + 借出中”定位最近一条可归还记录。
        var record = conn.QueryFirstOrDefault<BorrowRecord>(selectSql, new { studentId, isbn });
        if (record is null)
        {
            return OperationResult.Fail("未找到可归还记录");
        }

        var returnDate = DateTime.Now;
        // 仅按自然日计算超期天数（忽略时分秒）。
        var overdue = BorrowPolicy.CalculateOverdueDays(DateOnly.FromDateTime(record.DueDate), DateOnly.FromDateTime(returnDate));
        var status = overdue > 0 ? "超期" : "已归还";

        // 还书也是多表写入：更新借阅记录 + 回补库存，保持事务一致。
        using var tx = conn.BeginTransaction();
        const string updateRecord = @"
UPDATE borrow_record
SET return_date = @returnDate, status = @status, overdue_days = @overdue
WHERE record_id = @recordId";
        conn.Execute(updateRecord, new { returnDate, status, overdue, recordId = record.RecordId }, tx);
        conn.Execute("UPDATE book SET stock_qty = stock_qty + 1 WHERE isbn = @isbn", new { isbn }, tx);
        tx.Commit();

        _logs.Add(operatorUsername, "归还", $"学号:{studentId}, ISBN:{isbn}, 超期:{overdue}天");
        return OperationResult.Ok(overdue > 0 ? $"归还成功，超期 {overdue} 天" : "归还成功");
    }

    public int RefreshOverdueStatus(string operatorUsername)
    {
        using var conn = _factory.CreateOpenConnection();
        // 把“借出中且已过应还日”的记录批量置为“超期”。
        const string sql = @"
UPDATE borrow_record
SET status = '超期', overdue_days = DATEDIFF(CURDATE(), DATE(due_date))
WHERE status = '借出中' AND due_date < NOW()";

        var affected = conn.Execute(sql);
        if (affected > 0)
        {
            _logs.Add(operatorUsername, "超期提醒", $"更新超期记录 {affected} 条");
        }

        return affected;
    }
}

