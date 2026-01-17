using Garage.Extensions;

namespace Garage.ViewModels
{
    public class HomeViewModel
    {
        public float Capacity { get; init; }
        public int VehiclesParked { get; set; } = 0;
        public string PlacesLeft { get;set; } = string.Empty;
        public bool GarageIsFull { get; set; } = false;
    }
}
