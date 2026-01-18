using Garage.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Garage.ViewModels
{
    public class CreateOrEditViewModel
    {
        public int Id { get; set; }
        public string Registration { get; set; } = "";
        public string Color { get; set; } = "";
        public string Brand { get; set; } = "";
        public string Model { get; set; } = "";
        
        [Range(1, int.MaxValue, ErrorMessage = "Please select a vehicle type")]
        public int VehicleTypeId { get; set; }

        public VehicleType SelectedVehicleType { get; set; }
        [ValidateNever]
        public SelectList VehicleTypeList { get; set; }
        public bool GarageIsFull { get; set; } = false;
    }
}
