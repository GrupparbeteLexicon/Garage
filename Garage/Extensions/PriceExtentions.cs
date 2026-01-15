
namespace Garage.Extensions;

public static class PriceExtentions
{
    public static decimal HourlyRate { get; } = 20.00M;
    public static string Currency { get; } = "kr";

    // You pay every 30 minutes
    const decimal PayRate = 30.0M;

    // You can park for free for 2 hours (120 minutes)
    const decimal FreeTime = 120.0M;

    internal static decimal CalculateCost(TimeSpan totalParkedTime)
    {
        decimal costPerPeroid = PayRate * HourlyRate / 60.0M ;
        decimal totalCost = costPerPeroid * decimal.Ceiling(((decimal)totalParkedTime.TotalMinutes - FreeTime) / PayRate);
        return decimal.Max(totalCost, 0);
    }

    internal static decimal ParkedTimeToPrice(this TimeSpan totalParkedTime)
    {
        decimal costPerPeroid = PayRate * HourlyRate / 60.0M;
        decimal totalCost = costPerPeroid * decimal.Ceiling(((decimal)totalParkedTime.TotalMinutes - FreeTime) / PayRate);
        return decimal.Max(totalCost, 0);
    }
}
