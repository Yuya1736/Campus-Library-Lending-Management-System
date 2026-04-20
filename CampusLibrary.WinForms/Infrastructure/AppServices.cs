using CampusLibrary.Core.Data;
using CampusLibrary.Core.Repositories;
using CampusLibrary.Core.Services;

namespace CampusLibrary.WinForms.Infrastructure;

// 轻量级“服务容器”：
// 在 WinForms 项目中集中创建 Repository / Service，供各窗体复用。
public class AppServices
{
    public DbConnectionFactory Factory { get; }
    public BookService BookService { get; }
    public BorrowService BorrowService { get; }
    public AuthService AuthService { get; }
    public ReportService ReportService { get; }

    public AppServices(string connectionString)
    {
        // 所有仓储共用同一个连接工厂（按需创建连接）
        Factory = new DbConnectionFactory(connectionString);

        // 数据访问层
        var bookRepo = new BookRepository(Factory);
        var borrowRepo = new BorrowRepository(Factory);
        var userRepo = new UserRepository(Factory);
        var logRepo = new LogRepository(Factory);

        // 业务层
        BookService = new BookService(bookRepo, logRepo);
        BorrowService = new BorrowService(Factory, bookRepo, borrowRepo, logRepo);
        AuthService = new AuthService(userRepo, logRepo);
        ReportService = new ReportService(Factory, borrowRepo);
    }
}

