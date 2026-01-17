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
    }
}
