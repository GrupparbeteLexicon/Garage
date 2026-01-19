using Garage.Models;

namespace Garage.Data
{
    public class SeededParkingSpots
    {
        public static readonly IReadOnlyCollection<ParkingSpot> Default = new[]
        {
            new ParkingSpot
            {
                ParkingSpotSize = 1,
                Name = "M1" // Motorcycle spot
            },
            new ParkingSpot
            {
                ParkingSpotSize = 1,
                Name = "M2"
            },
            new ParkingSpot
            {
                ParkingSpotSize = 1,
                Name = "M3"
            },
            new ParkingSpot
            {
                ParkingSpotSize = 3,
                Name = "C1" // Car/ATV spot
            },
             new ParkingSpot
            {
                ParkingSpotSize = 3,
                Name = "C2"
            },
            new ParkingSpot
            {
                 ParkingSpotSize = 3,
                 Name = "C3"
            }
        };

        public static async Task SeedParkingSpots(IReadOnlyCollection<ParkingSpot> parkingSpots, GarageContext context)
        {
            foreach (var seed in parkingSpots)
            {
                var spotFound = context.ParkingSpots.FirstOrDefault(ps => ps.Name == seed.Name);
                if (spotFound == null)
                {
                    await context.ParkingSpots.AddAsync(seed);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
