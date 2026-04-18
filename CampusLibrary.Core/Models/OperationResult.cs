namespace CampusLibrary.Core.Models;

public class OperationResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;

    public static OperationResult Ok(string message = "操作成功") => new() { Success = true, Message = message };
    public static OperationResult Fail(string message) => new() { Success = false, Message = message };
}

