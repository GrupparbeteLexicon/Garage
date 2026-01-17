using Garage.Models;

namespace Garage.Services
{
    public interface IParkingCapacityService
    {
        Task<float> GetCapacityAsync();
        float CountPlaces(IEnumerable<Vehicle> vehicles);
    }
}
