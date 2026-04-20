using CampusLibrary.Core.Data;
using Dapper;

namespace CampusLibrary.Core.Repositories;

// 操作日志仓储：记录关键业务动作，便于审计与问题回溯。
public class LogRepository
{
    private readonly DbConnectionFactory _factory;

    public LogRepository(DbConnectionFactory factory)
    {
        _factory = factory;
    }

    public void Add(string operatorUsername, string operationType, string content, string ipAddress = "127.0.0.1")
    {
        using var conn = _factory.CreateOpenConnection();
        // 当前桌面端默认本机 IP；若接入网关可改为真实客户端地址。
        const string sql = @"
INSERT INTO operation_log(operator_username, operation_type, content, operation_time, ip_address)
VALUES(@operatorUsername, @operationType, @content, NOW(), @ipAddress)";
        conn.Execute(sql, new { operatorUsername, operationType, content, ipAddress });
    }
}

