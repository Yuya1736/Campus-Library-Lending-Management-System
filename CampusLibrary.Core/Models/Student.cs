namespace CampusLibrary.Core.Models;

// 学生实体：对应 student 表。
public class Student
{
    // 学号（主键）
    public string StudentId { get; set; } = string.Empty;
    // 姓名
    public string Name { get; set; } = string.Empty;
    // 班级
    public string ClassName { get; set; } = string.Empty;
    // 联系方式
    public string Contact { get; set; } = string.Empty;
    // 累计借阅次数
    public int TotalBorrowCount { get; set; }
}
