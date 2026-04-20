using CampusLibrary.Core.Repositories;
using Dapper;

namespace CampusLibrary.Core.Services;

// 报表服务：提供热门图书、分类借阅率、超期名单查询。
public class ReportService
{
    private readonly Data.DbConnectionFactory _factory;
    private readonly BorrowRepository _borrows;

    public ReportService(Data.DbConnectionFactory factory, BorrowRepository borrows)
    {
        _factory = factory;
        _borrows = borrows;
    }

    public List<dynamic> GetPopularBooks(int top = 10)
    {
        using var conn = _factory.CreateOpenConnection();
        // 按借阅次数倒序取 TopN，次数相同再按书名排序。
        const string sql = @"
SELECT isbn AS Isbn, title AS Title, borrow_count AS BorrowCount
FROM book
ORDER BY borrow_count DESC, title
LIMIT @top";
        return conn.Query(sql, new { top }).ToList();
    }

    public List<dynamic> GetBorrowRateByCategory()
    {
        using var conn = _factory.CreateOpenConnection();
        // 这里的 AvgBorrowCount = 总借阅次数 / 该分类图书数量。
        const string sql = @"
SELECT category AS Category,
       COUNT(*) AS BookCount,
       SUM(borrow_count) AS TotalBorrowCount,
       ROUND(SUM(borrow_count) / NULLIF(COUNT(*), 0), 2) AS AvgBorrowCount
FROM book
GROUP BY category
ORDER BY TotalBorrowCount DESC";
        return conn.Query(sql).ToList();
    }

    public List<Models.BorrowRecord> GetOverdueList()
    {
        return _borrows.GetOverdue();
    }
}

