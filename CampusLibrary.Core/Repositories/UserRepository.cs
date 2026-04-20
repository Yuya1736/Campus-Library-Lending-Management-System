using CampusLibrary.Core.Data;
using CampusLibrary.Core.Models;
using Dapper;

namespace CampusLibrary.Core.Repositories;

// 用户仓储：负责账号数据读写，不处理密码规则与权限规则。
public class UserRepository
{
    private readonly DbConnectionFactory _factory;

    public UserRepository(DbConnectionFactory factory)
    {
        _factory = factory;
    }

    public UserAccount? GetByUsername(string username)
    {
        using var conn = _factory.CreateOpenConnection();
        // 登录场景的核心查询：按用户名取出哈希密码和角色。
        const string sql = @"
SELECT user_id AS UserId, username AS Username, password_hash AS PasswordHash,
       role AS Role, last_login_time AS LastLoginTime
FROM user
WHERE username = @username";
        return conn.QueryFirstOrDefault<UserAccount>(sql, new { username });
    }

    public List<UserAccount> GetAll()
    {
        using var conn = _factory.CreateOpenConnection();
        const string sql = @"
SELECT user_id AS UserId, username AS Username, password_hash AS PasswordHash,
       role AS Role, last_login_time AS LastLoginTime
FROM user
ORDER BY user_id";
        return conn.Query<UserAccount>(sql).ToList();
    }

    public void Add(string username, string passwordHash, string role)
    {
        using var conn = _factory.CreateOpenConnection();
        // 密码以 hash 形式保存，不存明文。
        conn.Execute("INSERT INTO user(username, password_hash, role) VALUES(@username, @passwordHash, @role)",
            new { username, passwordHash, role });
    }

    public void Delete(int userId)
    {
        using var conn = _factory.CreateOpenConnection();
        conn.Execute("DELETE FROM user WHERE user_id = @userId", new { userId });
    }

    public void UpdateLastLoginTime(int userId)
    {
        using var conn = _factory.CreateOpenConnection();
        // 使用数据库 NOW()，避免客户端时间偏差。
        conn.Execute("UPDATE user SET last_login_time = NOW() WHERE user_id = @userId", new { userId });
    }
}

