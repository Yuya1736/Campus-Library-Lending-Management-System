using CampusLibrary.Core.Models;
using CampusLibrary.Core.Repositories;

namespace CampusLibrary.Core.Services;

// 认证与用户管理服务：负责登录、用户增删与操作日志记录。
public class AuthService
{
    private readonly UserRepository _users;
    private readonly LogRepository _logs;

    public AuthService(UserRepository users, LogRepository logs)
    {
        _users = users;
        _logs = logs;
    }

    public UserAccount? Login(string username, string password)
    {
        var user = _users.GetByUsername(username);
        if (user is null)
        {
            return null;
        }

        // 使用统一哈希算法校验密码，而非明文比对。
        if (!PasswordHasher.Verify(password, user.PasswordHash))
        {
            return null;
        }

        // 登录成功后更新最后登录时间并写操作日志。
        _users.UpdateLastLoginTime(user.UserId);
        _logs.Add(username, "登录", "用户登录系统");
        return _users.GetByUsername(username);
    }

    public List<UserAccount> GetUsers() => _users.GetAll();

    public OperationResult CreateUser(string operatorUsername, string username, string password, string role)
    {
        // 基本参数校验，避免空用户名/空密码入库。
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return OperationResult.Fail("用户名和密码不能为空");
        }

        var existing = _users.GetByUsername(username.Trim());
        if (existing is not null)
        {
            return OperationResult.Fail("用户名已存在");
        }

        _users.Add(username.Trim(), PasswordHasher.Hash(password.Trim()), role.Trim());
        _logs.Add(operatorUsername, "新增用户", $"用户名:{username}, 角色:{role}");
        return OperationResult.Ok("新增用户成功");
    }

    public OperationResult DeleteUser(string operatorUsername, int userId)
    {
        // 当前实现为物理删除，如需审计可改为逻辑删除。
        _users.Delete(userId);
        _logs.Add(operatorUsername, "删除用户", $"用户ID:{userId}");
        return OperationResult.Ok("删除用户成功");
    }
}

