using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;

namespace Garage.Models
{
    [Index(nameof(Registration), IsUnique = true)]
    public class ParkedVehicle
    {
            public int Id { get; set; }
            public VehicleType VehicleType { get; set; }

            [StringLength(6)]
            public string Registration { get; set; }
            public string Color { get; set; }

            [StringLength(30)]
            public string Brand { get; set; }

            [StringLength(30)]
            public string Model { get; set; }

            [Range(1, 16)]
            public int Wheels { get; set; }
            public DateTime ParkTime { get; set; }
    }
}
