using Garage.Models;
using System.ComponentModel.DataAnnotations;

namespace Garage.ViewModels
{
    public class ParkingVehicleViewModel
    {
        public int Id { get; }

        [Display(Name = "Vehicle Type")]
        public VehicleType VehicleType { get; }

        [Display(Name = "Registration Number")]
        public string Registration { get; }

        [Display(Name = "Color")]
        public string Color { get; }

        [Display(Name = "Brand")]
        public string Brand { get; }

        [Display(Name = "Model")]
        public string Model { get; }

        [Display(Name = "Parked Since")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? ParkTime { get; }

        [Display(Name = "Parked For")]
        public string ParkedDuration
        {
            get
            {
                if (ParkTime == null)
                    return "Not parked";

                var duration = DateTime.Now - ParkTime.Value;

                var hours = (int)duration.TotalHours;
                var minutes = duration.Minutes;

                return $"{hours:00}Hs {minutes:00}Mins";
            }
        }

        public ParkingVehicleViewModel(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            Id = vehicle.Id;
            Registration = vehicle.Registration;
            VehicleType = vehicle.VehicleType;
            ParkTime = vehicle.ParkingSpot?.ParkTime;
            Color = vehicle.Color;
            Brand = vehicle.Brand;
            Model = vehicle.Model;
        }
    }
}