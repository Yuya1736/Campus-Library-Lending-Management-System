namespace CampusLibrary.Core.Models;

// 用户账号实体：对应 user 表。
public class UserAccount
{
    // 用户ID（自增主键）
    public int UserId { get; set; }
    // 登录用户名
    public string Username { get; set; } = string.Empty;
    // 密码哈希（SHA256结果）
    public string PasswordHash { get; set; } = string.Empty;
    // 角色（管理员/操作员）
    public string Role { get; set; } = string.Empty;
    // 最后登录时间（首次登录前可能为空）
    public DateTime? LastLoginTime { get; set; }
}
