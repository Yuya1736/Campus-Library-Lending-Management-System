namespace CampusLibrary.Core.Models;

public class BorrowRecord
{
    public int RecordId { get; set; }
    public string Isbn { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int OverdueDays { get; set; }
}

