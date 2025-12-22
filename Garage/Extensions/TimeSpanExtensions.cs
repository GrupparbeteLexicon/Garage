namespace Garage.Extensions;

public static class TimeSpanExtensions
{
    public static string ParkedDurationString(this TimeSpan timeSpan)
    {
        if (timeSpan.TotalHours >= 1)
            return $"{(int)timeSpan.TotalHours}h, {timeSpan.Minutes:D2}m";
        return $"{timeSpan.Minutes:D2}m";
    }
}
