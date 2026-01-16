using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garage.Models;

[Index(nameof(Registration))]
[Index(nameof(OwnerId))]
[Index(nameof(ParkingSpotId))]
public class Vehicle
{
    public int Id { get; set; }

    public required int VehicleTypeId { get; set; }

    public required VehicleType VehicleType { get; set; }

    public required string OwnerId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    public required ApplicationUser Owner { get; set; }

    public int? ParkingSpotId { get; set; }

    [ForeignKey(nameof(ParkingSpotId))]
    public ParkingSpot? ParkingSpot { get; set; }

    [StringLength(6)]
    public string Registration { get; set; } = string.Empty;
    
    [StringLength(20)]
    public string Color { get; set; } = string.Empty;

    [StringLength(20)]
    public string Brand { get; set; } = string.Empty;

    [StringLength(20)]
    public string Model { get; set; } = string.Empty;

}
