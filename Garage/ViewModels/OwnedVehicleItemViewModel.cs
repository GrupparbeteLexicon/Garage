using Garage.Models;
using System.ComponentModel.DataAnnotations;

namespace Garage.ViewModels;

public class OwnedVehicleItemViewModel
{
    [Display(Name = "Vehicle Type")]
    public string VehicleType { get; }

    [Display(Name = "Registration Number")]
    public string Registration { get; }

    [Display(Name = "Color")]
    public string Color { get; }

    [Display(Name = "Brand")]
    public string Brand { get; }

    [Display(Name = "Model")]
    public string Model { get; }

    public bool IsParked { get; }

    [Display(Name = "Parked Since")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    public DateTime? ParkTime { get; }

    [Display(Name = "Parked For")]
    public TimeSpan? ParkedDuration { get; }

    public OwnedVehicleItemViewModel(Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);
        Registration = vehicle.Registration;
        VehicleType = vehicle.VehicleType.Name;
        ParkTime = vehicle.ParkingSpot?.ParkTime ?? null;
        Color = vehicle.Color;
        Brand = vehicle.Brand;
        Model = vehicle.Model;
        IsParked = vehicle.IsParked;
        ParkedDuration = DateTime.Now - ParkTime;
    }
}