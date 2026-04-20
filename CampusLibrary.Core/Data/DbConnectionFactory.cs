using System.Data;
using MySqlConnector;

namespace CampusLibrary.Core.Data;

// 数据库连接工厂：
// 每次调用 CreateOpenConnection 都返回一个已打开的 MySQL 连接。
public class DbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateOpenConnection()
    {
        var connection = new MySqlConnection(_connectionString);
        // 立即打开，调用方可以直接执行 Dapper 查询。
        connection.Open();
        return connection;
    }
}

