using System.Security.Cryptography;
using System.Text;

namespace CampusLibrary.Core.Services;

// 密码哈希工具：统一使用 SHA256，保证注册与登录校验算法一致。
public static class PasswordHasher
{
    public static string Hash(string password)
    {
        // 1) 把明文转为 UTF8 字节
        // 2) 计算 SHA256 摘要
        // 3) 转成十六进制字符串，便于数据库存储
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    public static bool Verify(string password, string hash)
    {
        // 重新计算输入密码的哈希，与库中哈希做不区分大小写比较。
        return string.Equals(Hash(password), hash, StringComparison.OrdinalIgnoreCase);
    }
}

