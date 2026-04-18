using CampusLibrary.Core.Data;
using CampusLibrary.Core.Models;
using Dapper;

namespace CampusLibrary.Core.Repositories;

public class BorrowRepository
{
    private readonly DbConnectionFactory _factory;

    public BorrowRepository(DbConnectionFactory factory)
    {
        _factory = factory;
    }

    public List<BorrowRecord> GetAll()
    {
        using var conn = _factory.CreateOpenConnection();
        const string sql = @"
SELECT record_id AS RecordId, isbn AS Isbn, student_id AS StudentId,
       borrow_date AS BorrowDate, due_date AS DueDate, return_date AS ReturnDate,
       status AS Status, overdue_days AS OverdueDays
FROM borrow_record
ORDER BY record_id DESC";
        return conn.Query<BorrowRecord>(sql).ToList();
    }

    public List<BorrowRecord> GetOverdue()
    {
        using var conn = _factory.CreateOpenConnection();
        const string sql = @"
SELECT record_id AS RecordId, isbn AS Isbn, student_id AS StudentId,
       borrow_date AS BorrowDate, due_date AS DueDate, return_date AS ReturnDate,
       status AS Status, overdue_days AS OverdueDays
FROM borrow_record
WHERE status = '超期'
ORDER BY due_date";
        return conn.Query<BorrowRecord>(sql).ToList();
    }
}

