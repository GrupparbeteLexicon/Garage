using Garage.Models;

namespace Garage.Data
{
    public static class SeededVehicleTypes
    {
        public static readonly IReadOnlyCollection<VehicleType> Default = new[]
        {
            new VehicleType
            {
                Name = "Motorcycle",
                VehicleSize = 1 // 1/3 Place
            },
            new VehicleType
            {
                Name = "ATV",
                VehicleSize = 3 // 1 Place
            },
            new VehicleType
            {
                Name = "Car",
                VehicleSize = 3 // 1 Place
            },
            new VehicleType
            {
                Name = "Airplane",
                VehicleSize = 9 // 3 Places
            },
            new VehicleType
            {
                Name = "Boat",
                VehicleSize = 9 // 3 Places 
            },
            new VehicleType
            {
                Name = "Bus",
                VehicleSize = 9 // 3 Places
            },
            new VehicleType
            {
                Name = "Truck",
                VehicleSize = 6 // 2 Places
            }
        };

        public static async Task SeedVehicleTypes(IReadOnlyCollection<VehicleType> vehicleTypes, GarageContext context)
        {
            foreach (var seed in vehicleTypes)
            {
                var typeFound = context.VehicleType.FirstOrDefault(vt => vt.Name == seed.Name); 
                if (typeFound == null)
                {
                    await context.VehicleType.AddAsync(seed);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}