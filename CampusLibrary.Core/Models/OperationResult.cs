namespace CampusLibrary.Core.Models;

// 统一业务返回对象：让 Service 向 UI 返回“是否成功 + 提示消息”。
public class OperationResult
{
    // 是否成功
    public bool Success { get; init; }
    // 结果说明（用于 MessageBox 提示）
    public string Message { get; init; } = string.Empty;

    // 成功工厂方法
    public static OperationResult Ok(string message = "操作成功") => new() { Success = true, Message = message };
    // 失败工厂方法
    public static OperationResult Fail(string message) => new() { Success = false, Message = message };
}
