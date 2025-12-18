using Garage.Controllers;
using Garage.Data;
using Garage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
	public class ParkedVehiclesControllerTests
	{
		// Creates a temporary in-memory database for each test
		private GarageContext GetDbContext()
		{
			var options = new DbContextOptionsBuilder<GarageContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			return new GarageContext(options);
		}

		[Fact]
		public async Task Index_Search_Filters_By_Registration()
		{
			// Add vehicles to the temporary database
			using var context = GetDbContext();
			context.ParkedVehicle.Add(new ParkedVehicle { Registration = "ABC123", Brand = "Volvo", Color = "Green", Model = "V70" });
			context.ParkedVehicle.Add(new ParkedVehicle { Registration = "XYZ999", Brand = "Saab", Color = "Blue", Model = "9-5" });
			await context.SaveChangesAsync();

			var controller = new ParkedVehiclesController(context);

			// Search for a vehicle with registration containing "ABC"
			var result = await controller.Index("ABC");

			// Verify that the method returns a ViewResult
			var viewResult = Assert.IsType<ViewResult>(result);

			// Verify that the ViewResult model is a list of vehicles
			var model = Assert.IsAssignableFrom<IEnumerable<ParkedVehicle>>(viewResult.Model);

			// Verify that the searched registration is found in the temporary database
			Assert.Equal("ABC123", model.First().Registration);
		}

		[Fact]
		public async Task Create_Adds_Vehicle_When_Model_Is_Valid()
		{
			using var context = GetDbContext();
			var controller = new ParkedVehiclesController(context);

			var vehicle = new ParkedVehicle
			{
				Registration = "NEW123",
				VehicleType = VehicleType.CAR,
				Color = "Red",
				Brand = "Volvo",
				Model = "XC60",
				Wheels = 4,
				ParkTime = DateTime.Now
			};

			// Call the Create method
			var result = await controller.Create(vehicle);

			// Verify that the database contains exactly one vehicle
			Assert.Single(context.ParkedVehicle);
		}

		[Fact]
		public async Task Create_Does_Not_Allow_Duplicate_Registration()
		{
			// Add a vehicle to the temporary database
			using var context = GetDbContext();
			context.ParkedVehicle.Add(new ParkedVehicle { Registration = "ABC123", Brand = "Volvo", Color = "Green", Model = "V70" });
			await context.SaveChangesAsync();

			var controller = new ParkedVehiclesController(context);

			var vehicle = new ParkedVehicle
			{
				Registration = "DUP123",
				VehicleType = VehicleType.CAR
			};

			// Call the Create method
			var result = await controller.Create(vehicle);

			// Verify that Create returns a ViewResult
			var viewResult = Assert.IsType<ViewResult>(result);

			// Verify that the controller's ModelState is invalid, because a vehicle with the same registration already exists
			Assert.False(controller.ModelState.IsValid);
		}
	}
}
