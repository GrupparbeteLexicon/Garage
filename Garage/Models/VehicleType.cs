using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Garage.Models;

public class VehicleType
{
	public string Name { get; set; } = string.Empty;
    public int Id { get; set; }
	public int VehicleSize { get; set; }

}