using CampusLibrary.Core.Models;
using CampusLibrary.Core.Repositories;

namespace CampusLibrary.Core.Services;

public class BookService
{
    private readonly BookRepository _books;
    private readonly LogRepository _logs;

    public BookService(BookRepository books, LogRepository logs)
    {
        _books = books;
        _logs = logs;
    }

    public List<Book> GetBooks() => _books.GetAll();

    public OperationResult AddBook(string operatorUsername, Book book)
    {
        if (string.IsNullOrWhiteSpace(book.Isbn) || string.IsNullOrWhiteSpace(book.Title))
        {
            return OperationResult.Fail("ISBN和书名不能为空");
        }

        if (_books.GetByIsbn(book.Isbn) is not null)
        {
            return OperationResult.Fail("ISBN已存在");
        }

        if (book.StockQty < 0)
        {
            return OperationResult.Fail("库存不能小于0");
        }

        book.BorrowCount = 0;
        _books.Add(book);
        _logs.Add(operatorUsername, "新增图书", $"ISBN:{book.Isbn}, 书名:{book.Title}");
        return OperationResult.Ok("新增图书成功");
    }

    public OperationResult UpdateBook(string operatorUsername, Book book)
    {
        if (book.StockQty < 0)
        {
            return OperationResult.Fail("库存不能小于0");
        }

        _books.Update(book);
        _logs.Add(operatorUsername, "修改图书", $"ISBN:{book.Isbn}, 书名:{book.Title}");
        return OperationResult.Ok("修改图书成功");
    }

    public OperationResult DeleteBook(string operatorUsername, string isbn)
    {
        _books.Delete(isbn);
        _logs.Add(operatorUsername, "删除图书", $"ISBN:{isbn}");
        return OperationResult.Ok("删除图书成功");
    }
}

