using Garage.Models;

namespace Garage.ViewModels
{
    public class ReceiptViewModel
    {
        public string Registration { get; set; } = string.Empty;
        public VehicleType VehicleType { get; set; }
        public DateTime? ParkTime { get; set; }
        public DateTime? LeaveTime { get; set; }
        public TimeSpan TotalParkedTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string Currency { get; set; }
    }
}