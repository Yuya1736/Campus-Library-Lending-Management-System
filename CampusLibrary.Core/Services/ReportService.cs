using CampusLibrary.Core.Repositories;
using Dapper;

namespace CampusLibrary.Core.Services;

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

