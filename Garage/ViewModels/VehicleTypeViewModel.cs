using Garage.Extensions;
using Garage.Models;
using System.Drawing;

namespace Garage.ViewModels
{
    public class VehicleTypeViewModel
	{

		public VehicleTypeViewModel(VehicleType vehicleType)
		{
			ArgumentNullException.ThrowIfNull(vehicleType);
			Id = vehicleType.Id;
			Name = vehicleType.Name;
			ParkSize = ""; // TODO: Map to vehicleType ParkSize
		}

		public int Id { get; }
		public string Name { get; }
		public string ParkSize { get; }

	}
}
