using Garage.Models;
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
        public int VehicleTypeId { get; set; }

        public VehicleType SelectedVehicleType { get; set; }
        public SelectList VehicleTypeList { get; set; }
        public bool GarageIsFull { get; set; } = false;
    }
}
