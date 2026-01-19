using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage.ViewModels
{
    public class ParkingIndexViewModel
    {
        public IReadOnlyList<ParkingVehicleViewModel> Vehicles { get; init; } = [];

        public float Capacity { get; init; }

        public int VehiclesParked => Vehicles.Count;

        public float PlacesUsed { get; init; }

        public string PlacesLeft { get; init; }

        public bool IsFull => PlacesUsed >= Capacity;
        public string? Search { get; init; } = string.Empty;
        public int? VehicleTypeId { get; set; }
        public SelectList VehicleTypeList { get; set; }
    }
}