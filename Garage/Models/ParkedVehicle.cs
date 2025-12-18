using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garage.Models;

[Index(nameof(Registration), IsUnique = true)]
public class ParkedVehicle
{
    public int Id { get; set; }

    [Display(Name = "Vehicle Type")]
    public VehicleType VehicleType { get; set; }

    [StringLength(6)]
    [Display(Name = "Registration Number")]
    public string Registration { get; set; }
    
    [StringLength(20)]
    [Display(Name = "Color")]
    public string Color { get; set; }
    
    [StringLength(20)]
    [Display(Name = "Brand")]
    public string Brand { get; set; }
    
    [StringLength(20)]
    [Display(Name = "Model")]
    public string Model { get; set; }
    
    [Range(1, 18)]
    [Display(Name = "Number of Wheels")]
    public int Wheels { get; set; }

    [Display(Name = "Parked Since")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    public DateTime ParkTime { get; set; } = DateTime.Now;

    [Display(Name = "Parked For")]
    [NotMapped]
    public TimeSpan ParkedDuration => DateTime.Now - ParkTime;
}
