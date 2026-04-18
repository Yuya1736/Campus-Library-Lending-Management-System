namespace CampusLibrary.Core.Models;

public class OperationLog
{
    public long LogId { get; set; }
    public string OperatorUsername { get; set; } = string.Empty;
    public string OperationType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime OperationTime { get; set; }
    public string IpAddress { get; set; } = string.Empty;
}

