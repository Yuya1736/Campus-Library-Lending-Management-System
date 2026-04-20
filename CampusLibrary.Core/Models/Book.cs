namespace CampusLibrary.Core.Models;

// 图书实体：对应数据库 book 表。
public class Book
{
    // 国际标准书号（主键）
    public string Isbn { get; set; } = string.Empty;
    // 书名
    public string Title { get; set; } = string.Empty;
    // 作者
    public string Author { get; set; } = string.Empty;
    // 出版社
    public string Publisher { get; set; } = string.Empty;
    // 分类（如：计算机、数据库）
    public string Category { get; set; } = string.Empty;
    // 当前可借库存
    public int StockQty { get; set; }
    // 历史累计借阅次数（用于热门图书统计）
    public int BorrowCount { get; set; }
}
