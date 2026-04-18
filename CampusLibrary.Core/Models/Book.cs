namespace CampusLibrary.Core.Models;

public class Book
{
    public string Isbn { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int StockQty { get; set; }
    public int BorrowCount { get; set; }
}

