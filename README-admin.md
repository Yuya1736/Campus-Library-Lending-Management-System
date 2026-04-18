# 校园图书借阅管理系统

本项目基于 `.NET 8 + WinForms + MySQL` 开发，支持图书管理、借阅归还、超期提醒、统计分析和用户权限管理。

## 功能说明

- 图书管理：新增、修改、删除、查询
- 借阅归还：按学号和 ISBN 进行借还
- 超期提醒：刷新并更新超期状态
- 统计分析：热门图书、分类借阅率
- 权限管控：管理员/操作员账号管理
- 登录页数据库状态提示：显示数据库是否连接成功

## 技术栈

- .NET 8
- WinForms
- MySQL 8.x
- Dapper
- xUnit

## 项目结构

```text
Campus_Library_Lending_Management_System/
|- CampusLibrary.Core/       # 领域模型、仓储、服务、数据库连接
|- CampusLibrary.WinForms/   # WinForms 界面与程序入口
|- CampusLibrary.Tests/      # 单元测试
|- database/
|  |- schema.sql             # 建表脚本
|  `- seed.sql               # 初始化数据脚本
`- CampusLibrary.slnx
```

## 运行环境

- Windows（用于运行 WinForms 客户端）
- .NET SDK 8.0+
- MySQL Server 8.0+

## 快速开始

### 1. 创建数据库结构

执行 `database/schema.sql`：

```bash
mysql -u <user> -p < database/schema.sql
```

### 2. 导入初始数据

执行 `database/seed.sql`：

```bash
mysql -u <user> -p campus_library < database/seed.sql
```

### 3. 配置连接字符串

编辑 `CampusLibrary.WinForms/Config/AppConfig.cs` 中的 `ConnectionString`，示例：

```csharp
"Server=127.0.0.1;Port=3306;Database=campus_library;Uid=campus_app;Pwd=your_password;CharSet=utf8mb4;AllowUserVariables=True"
```

如果使用远程数据库，把 `Server` 改为服务器 IP（例如 `154.12.94.94`）。

### 4. 运行程序

```bash
dotnet run --project CampusLibrary.WinForms/CampusLibrary.WinForms.csproj
```

## 默认账号（来自 seed.sql）

- 管理员：`admin / 123456`
- 操作员：`operator / operator123`

## 构建与测试

```bash
dotnet build CampusLibrary.slnx
dotnet test CampusLibrary.slnx
```

## 常见问题

### 无法连接数据库

典型报错：
`Unable to connect to any of the specified MySQL hosts`

请检查：

- 连接字符串中的 IP、端口、用户名、密码是否正确
- MySQL 服务状态：`systemctl status mysql`
- MySQL 监听地址（`bind-address`）是否允许远程访问
- 服务器防火墙和云安全组是否放行 `3306/tcp`
- 数据库账号 host 是否允许远程（例如 `'campus_app'@'%'`）

### 登录页提示数据库连接失败

- 先在客户端机器测试连通性：`mysql -h <ip> -P 3306 -u <user> -p`
- 再确认 `AppConfig.cs` 的连接字符串已修改并保存

## 说明

- 当前数据库连接字符串写在代码中，适合课程/演示场景。
- 生产环境建议改为 `appsettings.json` 或环境变量管理敏感信息。
