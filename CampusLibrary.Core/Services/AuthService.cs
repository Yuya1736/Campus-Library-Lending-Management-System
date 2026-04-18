using CampusLibrary.Core.Models;
using CampusLibrary.Core.Repositories;

namespace CampusLibrary.Core.Services;

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

        if (!PasswordHasher.Verify(password, user.PasswordHash))
        {
            return null;
        }

        _users.UpdateLastLoginTime(user.UserId);
        _logs.Add(username, "登录", "用户登录系统");
        return _users.GetByUsername(username);
    }

    public List<UserAccount> GetUsers() => _users.GetAll();

    public OperationResult CreateUser(string operatorUsername, string username, string password, string role)
    {
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
        _users.Delete(userId);
        _logs.Add(operatorUsername, "删除用户", $"用户ID:{userId}");
        return OperationResult.Ok("删除用户成功");
    }
}

