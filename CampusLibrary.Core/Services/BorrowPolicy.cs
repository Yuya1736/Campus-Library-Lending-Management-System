namespace CampusLibrary.Core.Services;

public static class BorrowPolicy
{
    public static int CalculateOverdueDays(DateOnly dueDate, DateOnly returnDate)
    {
        var days = returnDate.DayNumber - dueDate.DayNumber;
        return Math.Max(0, days);
    }

    public static bool IsOverdue(DateOnly dueDate, DateOnly today)
    {
        return today.DayNumber > dueDate.DayNumber;
    }
}

