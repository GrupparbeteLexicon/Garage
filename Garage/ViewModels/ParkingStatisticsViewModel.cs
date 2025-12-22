using Garage.Models;

namespace Garage.ViewModels;

public class ParkingStatisticsViewModel
{
    public int Capacity { get; set; }
    public int PlacesUsed { get; set; }
    public string PlacesLeft { get; set; }
    public string Currency { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal TotalParkedTime { get; set; }
    public decimal TotalRevenue { get; set; }
    public Dictionary<string, int> VehicleTypeCounts { get; set; }
}
