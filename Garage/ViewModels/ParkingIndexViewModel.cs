namespace Garage.ViewModels
{
    public class ParkingIndexViewModel
    {
        public IReadOnlyList<ParkingVehicleViewModel> Vehicles { get; init; } = [];

        public float Capacity { get; init; }

        public int VehiclesParked => Vehicles.Count;

        public float PlacesUsed { get; init; }

        public float PlacesLeft => Capacity - PlacesUsed;

        public bool IsFull => PlacesUsed >= Capacity;
    }
}
