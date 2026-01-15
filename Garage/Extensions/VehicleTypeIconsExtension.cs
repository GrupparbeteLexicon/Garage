using Garage.Models;

namespace Garage.Extensions
{
    public static class VehicleTypeExtensions
    {
        public static string ToIconClass(this VehicleTypeModel vehicleType)
        {
            return vehicleType switch
            {
                VehicleTypeModel.CAR => "bi-car-front-fill",
                VehicleTypeModel.MOTORCYCLE => "bi-bicycle",
                VehicleTypeModel.ATV => "bi-truck",
                VehicleTypeModel.BUS => "bi-bus-front-fill",
                VehicleTypeModel.TRUCK => "bi-truck-front-fill",
                VehicleTypeModel.AIRPLANE => "bi-airplane-fill",
                VehicleTypeModel.BOAT => "bi-boat-fill",
                _ => "bi-question-circle"
            };
        }
    }
}