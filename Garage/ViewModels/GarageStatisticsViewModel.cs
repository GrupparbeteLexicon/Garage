using Garage.Models;
using System.ComponentModel.DataAnnotations;

namespace Garage.ViewModels
{
    public class GarageStatisticsViewModel
    {
        [Display(Name = "Number of each type of Vehicle:")]
        public int TotalVehicles { get; set; }

        public Dictionary<VehicleType, int> VehiclesByType { get; set; }
            = new();

        public int TotalWheels { get; set; }

        public TimeSpan TotalParkedTime { get; set; }

    }
}