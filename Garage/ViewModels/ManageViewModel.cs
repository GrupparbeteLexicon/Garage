using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage.ViewModels
{
    public class ManageViewModel
    {
        public IReadOnlyList<ManageVehicleViewModel> Vehicles { get; init; }

        public SelectList VehicleTypeList { get; set; }
        public int? VehicleTypeId { get; set; }
        public string? Search { get; set; }
    }
}
