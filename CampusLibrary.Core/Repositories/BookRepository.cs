using CampusLibrary.Core.Data;
using CampusLibrary.Core.Models;
using Dapper;

namespace CampusLibrary.Core.Repositories;

// 图书仓储：只负责“数据读写”，不做业务规则判断。
// 业务校验（如库存不能小于0）由 Service 层完成。
public class BookRepository
{
    private readonly DbConnectionFactory _factory;

    public BookRepository(DbConnectionFactory factory)
    {
        _factory = factory;
    }

    public List<Book> GetAll()
    {
        using var conn = _factory.CreateOpenConnection();
        // AS 别名用于把数据库下划线命名映射到 C# PascalCase 属性。
        const string sql = @"
SELECT isbn AS Isbn, title AS Title, author AS Author, publisher AS Publisher,
       category AS Category, stock_qty AS StockQty, borrow_count AS BorrowCount
FROM book
ORDER BY title";
        return conn.Query<Book>(sql).ToList();
    }

    public Book? GetByIsbn(string isbn)
    {
        using var conn = _factory.CreateOpenConnection();
        // QueryFirstOrDefault：查不到返回 null，不抛“无数据”异常。
        const string sql = @"
SELECT isbn AS Isbn, title AS Title, author AS Author, publisher AS Publisher,
       category AS Category, stock_qty AS StockQty, borrow_count AS BorrowCount
FROM book
WHERE isbn = @isbn";
        return conn.QueryFirstOrDefault<Book>(sql, new { isbn });
    }

    public void Add(Book book)
    {
        using var conn = _factory.CreateOpenConnection();
        // 参数名与对象属性同名时，Dapper 会自动绑定。
        const string sql = @"
INSERT INTO book(isbn, title, author, publisher, category, stock_qty, borrow_count)
VALUES(@Isbn, @Title, @Author, @Publisher, @Category, @StockQty, @BorrowCount)";
        conn.Execute(sql, book);
    }

    public void Update(Book book)
    {
        using var conn = _factory.CreateOpenConnection();
        // 通过主键 ISBN 定位更新目标行。
        const string sql = @"
UPDATE book
SET title = @Title, author = @Author, publisher = @Publisher, category = @Category, stock_qty = @StockQty
WHERE isbn = @Isbn";
        conn.Execute(sql, book);
    }

    public void Delete(string isbn)
    {
        using var conn = _factory.CreateOpenConnection();
        conn.Execute("DELETE FROM book WHERE isbn = @isbn", new { isbn });
    }
}

