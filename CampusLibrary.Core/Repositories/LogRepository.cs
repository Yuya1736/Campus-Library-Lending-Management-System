using CampusLibrary.Core.Data;
using Dapper;

namespace CampusLibrary.Core.Repositories;

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
        const string sql = @"
INSERT INTO operation_log(operator_username, operation_type, content, operation_time, ip_address)
VALUES(@operatorUsername, @operationType, @content, NOW(), @ipAddress)";
        conn.Execute(sql, new { operatorUsername, operationType, content, ipAddress });
    }
}

