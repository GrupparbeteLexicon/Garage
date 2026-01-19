using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Garage.Models;

[Index(nameof(ParkTime))]
[Index(nameof(Blocked), nameof(ParkedVehicleID))]
public class ParkingSpot
{
    public int Id { get; set; }

    public int? ParkedVehicleID { get; set;}

    public Vehicle? ParkedVehicle { get; set; }

    public int ParkingSpotSize { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool Blocked { get; set; } = false;

    public DateTime? ParkTime { get; set; } = null;
}