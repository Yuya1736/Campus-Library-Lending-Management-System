namespace CampusLibrary.WinForms.Config;

public static class AppConfig
{
    // TODO: 首次运行请改成本机 MySQL 信息。
    // 说明：当前写法便于课程演示；生产环境建议放到配置文件或环境变量中。
    public static string ConnectionString { get; set; } =
        "Server=154.12.94.94;Port=3306;Database=campus_library;Uid=campus_app;Pwd=root;CharSet=utf8mb4;AllowUserVariables=True"
;
}
