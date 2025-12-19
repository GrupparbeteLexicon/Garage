using Garage.Models;

namespace Garage.ViewModels;

public class ParkingStatisticsViewModel
{
    public int Capacity { get; set; }
    public int Count { get; set; }
    public string Currency { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal TotalParkedTime { get; set; }
    public Dictionary<VehicleType, int> VehicleTypeCounts { get; set; }
}
