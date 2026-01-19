using Garage.Models;

namespace Garage.ViewModels;

public class OwnedVehiclesViewModel
{
    public List<OwnedVehicleItemViewModel> Vehicles { get; } = [];

    public OwnedVehiclesViewModel(ApplicationUser user)
    {
        ArgumentNullException.ThrowIfNull(user);

        Vehicles = [.. user.OwnedVehicles.Select(vehicle => new OwnedVehicleItemViewModel(vehicle))];
    }
}