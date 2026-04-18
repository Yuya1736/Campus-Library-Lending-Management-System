using CampusLibrary.Core.Services;

namespace CampusLibrary.Tests;

public class BorrowPolicyTests
{
    [Fact]
    public void CalculateOverdueDays_WhenReturnedLate_ShouldReturnPositiveDays()
    {
        var dueDate = new DateOnly(2026, 4, 10);
        var returned = new DateOnly(2026, 4, 14);

        var overdueDays = BorrowPolicy.CalculateOverdueDays(dueDate, returned);

        Assert.Equal(4, overdueDays);
    }

    [Fact]
    public void CalculateOverdueDays_WhenReturnedOnTime_ShouldReturnZero()
    {
        var dueDate = new DateOnly(2026, 4, 10);
        var returned = new DateOnly(2026, 4, 10);

        var overdueDays = BorrowPolicy.CalculateOverdueDays(dueDate, returned);

        Assert.Equal(0, overdueDays);
    }

    [Fact]
    public void IsOverdue_WhenDateHasPassedAndNotReturned_ShouldBeTrue()
    {
        var dueDate = new DateOnly(2026, 4, 10);
        var today = new DateOnly(2026, 4, 18);

        var isOverdue = BorrowPolicy.IsOverdue(dueDate, today);

        Assert.True(isOverdue);
    }
}

