namespace CampusLibrary.Core.Models;

// 借阅记录实体：对应 borrow_record 表，一条记录代表一次借阅生命周期。
public class BorrowRecord
{
    // 自增主键
    public int RecordId { get; set; }
    // 图书 ISBN（外键到 book）
    public string Isbn { get; set; } = string.Empty;
    // 学号（外键到 student）
    public string StudentId { get; set; } = string.Empty;
    // 借出时间
    public DateTime BorrowDate { get; set; }
    // 应还时间
    public DateTime DueDate { get; set; }
    // 实际归还时间（未还时为空）
    public DateTime? ReturnDate { get; set; }
    // 状态：借出中/已归还/超期
    public string Status { get; set; } = string.Empty;
    // 超期天数（未超期为0）
    public int OverdueDays { get; set; }
}
