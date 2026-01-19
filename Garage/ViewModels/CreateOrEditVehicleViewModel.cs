using Garage.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garage.ViewModels;
public class CreateOrEditVehicleViewModel
{
    public int VehicleTypeId { get; set; }
    public int? ParkingSpotId { get; set; }
    [StringLength(6)]
    public string Registration { get; set; } = string.Empty;

    [StringLength(20)]
    public string Color { get; set; } = string.Empty;

    [StringLength(20)]
    public string Brand { get; set; } = string.Empty;

    [StringLength(20)]
    public string Model { get; set; } = string.Empty;

    public CreateOrEditVehicleViewModel()
    {
    }
    public CreateOrEditVehicleViewModel(Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);
        Registration = vehicle.Registration;
        VehicleTypeId = vehicle.VehicleTypeId;
        ParkingSpotId = vehicle.ParkingSpotId;
        Color = vehicle.Color;
        Brand = vehicle.Brand;
        Model = vehicle.Model;
    }
}
