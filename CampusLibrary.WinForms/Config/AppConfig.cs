namespace CampusLibrary.WinForms.Config;

public static class AppConfig
{
    // TODO: 首次运行请改成本机 MySQL 信息。
    public static string ConnectionString { get; set; } =
        "Server=154.12.94.94;Port=3306;Database=campus_library;Uid=campus_app;Pwd=root;CharSet=utf8mb4;AllowUserVariables=True"
;
}

