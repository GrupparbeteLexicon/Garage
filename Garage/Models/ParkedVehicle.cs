using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Garage.Models;

public class ParkedVehicle
{
    public int Id { get; set; }

    [Display(Name = "Vehicle Type")]
    public VehicleType VehicleType { get; set; }

    [Display(Name = "Registration Number")]
    public string Registration { get; set; }

    [Display(Name = "Color")]
    public string Color { get; set; }

    [Display(Name = "Brand")]
    public string Brand { get; set; }

    [Display(Name = "Model")]
    public string Model { get; set; }

    [Display(Name = "Number of Wheels")]
    public int Wheels { get; set; }

    [Display(Name = "Parked Since")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    public DateTime ParkTime { get; set; } = DateTime.Now;

    [Display(Name = "Parked For")]
    [NotMapped]
    public TimeSpan ParkedDuration => DateTime.Now - ParkTime;
}