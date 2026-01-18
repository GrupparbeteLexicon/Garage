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
			ParkSize = vehicleType.ParkSize;
		}

		public int Id { get; }
		public string Name { get; }
		public int ParkSize { get; }

	}
}
