using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Garage.Models;

[Index(nameof(Registration), IsUnique = true)]
public class ParkedVehicle
{
    public int Id { get; set; }

    public VehicleType VehicleType { get; set; }

    [StringLength(6)]
    public string Registration { get; set; }
    
    [StringLength(20)]
    public string Color { get; set; }
    
    [StringLength(20)]
    public string Brand { get; set; }
    
    [StringLength(20)]
    public string Model { get; set; }
    
    [Range(1, 18)]
    public int Wheels { get; set; }

    public DateTime ParkTime { get; set; } = DateTime.Now;

}
