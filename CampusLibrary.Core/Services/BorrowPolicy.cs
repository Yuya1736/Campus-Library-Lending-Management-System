namespace CampusLibrary.Core.Services;

// 纯规则类：不依赖数据库，便于单元测试。
public static class BorrowPolicy
{
    public static int CalculateOverdueDays(DateOnly dueDate, DateOnly returnDate)
    {
        // 逾期天数 = 归还日 - 应还日，小于 0 则记为 0。
        var days = returnDate.DayNumber - dueDate.DayNumber;
        return Math.Max(0, days);
    }

    public static bool IsOverdue(DateOnly dueDate, DateOnly today)
    {
        return today.DayNumber > dueDate.DayNumber;
    }
}
