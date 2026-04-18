# 校园图书借阅管理系统

本说明面向课程同学使用客户端，不需要自行部署数据库。

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

## 你需要准备的内容

- Windows 电脑
- .NET SDK 8.0 或更高版本
- 项目代码（本仓库）
- 管理员提供的数据库连接信息：
  - 数据库服务器 IP（现为 `154.12.94.94`）
  - 端口（现为 `3306`）
  - 数据库名（通常是 `campus_library`）
  - 数据库账号和密码（例如 `root`）

## 快速开始

### 1. 修改数据库连接字符串

打开文件：

- `CampusLibrary.WinForms/Config/AppConfig.cs`

把 `ConnectionString` 改成老师/管理员给你的信息，例如：

```csharp
"Server=154.12.94.94;Port=3306;Database=campus_library;Uid=campus_app;Pwd=root;CharSet=utf8mb4;AllowUserVariables=True"
```

### 2. 运行程序

在项目根目录执行：

```bash
dotnet run --project CampusLibrary.WinForms/CampusLibrary.WinForms.csproj
```

或直接在VS中运行 CampusLibrary.WinForms.csproj

### 3. 登录系统

如果管理员使用了默认种子数据，可用：

- 管理员：`admin / 123456`
- 操作员：`operator / operator123`

如果管理员改过账号密码，请以管理员提供为准。

## 常见问题

### 登录页提示“数据库连接失败”

请优先检查：

- `AppConfig.cs` 的 `Server/Port/Uid/Pwd` 是否填对
- 你当前网络是否能访问数据库服务器
- 管理员是否已开放服务器 `3306` 端口给你的网络

### 报错：Unable to connect to any of the specified MySQL hosts

这通常是网络或权限问题，不是你代码问题。请把以下信息发给管理员：

- 你的连接字符串（可隐藏密码）
- 报错完整截图
- 你的公网 IP（方便管理员加白名单）

## 开发命令（可选）

```bash
dotnet build CampusLibrary.slnx
dotnet test CampusLibrary.slnx
```

## 管理员文档

部署数据库、初始化数据、服务器配置等内容，请查看：

- `README-admin.md`
