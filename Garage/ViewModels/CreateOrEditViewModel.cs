using Garage.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage.ViewModels
{
    public class CreateOrEditViewModel
    {
        
        public VehicleType SelectedVehicleType { get; set; }
        public SelectList VehicleTypeList { get; set; }
        public ParkedVehicle? ParkedVehicle { get; set;  }
        public bool DisableEditParkTime { get; set; } = false;
    }
}
