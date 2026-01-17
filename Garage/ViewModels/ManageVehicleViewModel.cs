using Garage.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Garage.ViewModels
{
    public class ManageVehicleViewModel
    {
        public int Id { get; }

        [Display(Name = "Vehicle Type")]
        public string VehicleType { get; }

        [Display(Name = "Owner")]
        public string Owner { get; set; }

        [Display(Name = "Parking Spot")]
        public string ParkingSpot { get; set; }

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

        public ManageVehicleViewModel(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);
            Id = vehicle.Id;
            VehicleType = vehicle.VehicleType.Name;
            Owner = vehicle.Owner.FullName;
            ParkingSpot = vehicle.ParkingSpot?.Name ?? "Not Parked";
            Registration = vehicle.Registration;
            ParkTime = vehicle.ParkingSpot?.ParkTime;
            Color = vehicle.Color;
            Brand = vehicle.Brand;
            Model = vehicle.Model;
        }
    }
}
