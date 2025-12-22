using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Garage.Models;

public enum VehicleType {
    [Display(Name = "Car")]
    CAR,

    [Display(Name = "Motorcycle")]
    MOTORCYCLE,

    [Display(Name = "ATV")]
    ATV,

    [Display(Name = "Bus")]
    BUS,

    [Display(Name = "Truck")]
    TRUCK,

    [Display(Name = "Airplane")]
    AIRPLANE,

    [Display(Name = "Boat")]
    BOAT
}