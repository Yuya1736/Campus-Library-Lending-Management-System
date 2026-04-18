using CampusLibrary.Core.Data;
using CampusLibrary.Core.Repositories;
using CampusLibrary.Core.Services;

namespace CampusLibrary.WinForms.Infrastructure;

public class AppServices
{
    public DbConnectionFactory Factory { get; }
    public BookService BookService { get; }
    public BorrowService BorrowService { get; }
    public AuthService AuthService { get; }
    public ReportService ReportService { get; }

    public AppServices(string connectionString)
    {
        Factory = new DbConnectionFactory(connectionString);

        var bookRepo = new BookRepository(Factory);
        var borrowRepo = new BorrowRepository(Factory);
        var userRepo = new UserRepository(Factory);
        var logRepo = new LogRepository(Factory);

        BookService = new BookService(bookRepo, logRepo);
        BorrowService = new BorrowService(Factory, bookRepo, borrowRepo, logRepo);
        AuthService = new AuthService(userRepo, logRepo);
        ReportService = new ReportService(Factory, borrowRepo);
    }
}

