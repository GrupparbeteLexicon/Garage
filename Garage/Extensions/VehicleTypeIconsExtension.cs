using Garage.Models;

namespace Garage.Extensions
{
    public static class VehicleTypeExtensions
    {
        public static string ToIconClass(this VehicleType vehicleType)
        {
            return vehicleType switch
            {
                VehicleType.CAR => "bi-car-front-fill",
                VehicleType.MOTORCYCLE => "bi-bicycle",
                VehicleType.ATV => "bi-truck",
                VehicleType.BUS => "bi-bus-front-fill",
                VehicleType.TRUCK => "bi-truck-front-fill",
                VehicleType.AIRPLANE => "bi-airplane-fill",
                VehicleType.BOAT => "bi-boat-fill",
                _ => "bi-question-circle"
            };
        }
    }
}