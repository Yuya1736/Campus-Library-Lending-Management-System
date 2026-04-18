namespace CampusLibrary.Core.Models;

public class Student
{
    public string StudentId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public int TotalBorrowCount { get; set; }
}

