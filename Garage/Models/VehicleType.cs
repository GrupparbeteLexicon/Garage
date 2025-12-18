using System.ComponentModel.DataAnnotations;

namespace Garage.Models;

public enum VehicleType {
    [Display(Name = "Car")]
    CAR,

    [Display(Name = "Motorcycle")]
    MOTORCYCLE,

    [Display(Name = "ATV")]
    ATV,

    [Display(Name = "Buss")]
    BUSS,

    [Display(Name = "Truck")]
    TRUCK 
}