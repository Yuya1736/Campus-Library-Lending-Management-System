namespace CampusLibrary.Core.Models;

// 操作日志实体：对应 operation_log 表。
public class OperationLog
{
    // 日志ID（自增主键）
    public long LogId { get; set; }
    // 操作人用户名
    public string OperatorUsername { get; set; } = string.Empty;
    // 操作类型（如：登录、借阅、删除图书）
    public string OperationType { get; set; } = string.Empty;
    // 具体内容（业务上下文）
    public string Content { get; set; } = string.Empty;
    // 操作时间
    public DateTime OperationTime { get; set; }
    // 操作来源IP
    public string IpAddress { get; set; } = string.Empty;
}
