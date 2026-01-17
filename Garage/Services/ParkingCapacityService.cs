using Garage.Data;
using Garage.Models;
using Microsoft.EntityFrameworkCore;

namespace Garage.Services
{
    public class ParkingCapacityService : IParkingCapacityService
    {
        private readonly GarageContext _context;

        public ParkingCapacityService(GarageContext context)
        {
            _context = context;
        }

        public async Task<float> GetCapacityAsync()
        {
            return await _context.ParkingSpots.CountAsync();
        }

        public static float CountPlaces(IQueryable<Vehicle> vehicles)
        {
            int placesUsed = 0;

            int parkedVehiclesCount = vehicles.Where(v => v.ParkingSpotId != null).ToList().Count();


            foreach (var vehicle in vehicles)
            {
                placesUsed += vehicle.VehicleType.VehicleSize;
            }

            return placesUsed / 3f; // Since 1 Place = 3 Units
        }

        public float CountPlaces(IEnumerable<Vehicle> vehicles) 
        {
            int placesUsed = 0;

            foreach (var vehicle in vehicles.Where(v => v.ParkingSpotId != null))
            {
                placesUsed += vehicle.VehicleType.VehicleSize;
            }

            return placesUsed / 3f; // 1 place = 3 units
        }

        public static string ToMixedFraction(float value, int maxDenominator = 3) // using maxDenominator 3 because we are interested in thirds
        {
            int numerator = (int)Math.Round(value * maxDenominator);
            int denominator = maxDenominator;

            int gcd = GCD(numerator, denominator);
            numerator /= gcd;
            denominator /= gcd;

            // whole number part
            int whole = numerator / denominator;
            int remainder = numerator % denominator;

            if (whole > 0 && remainder > 0)
                return $"{whole} and {remainder}/{denominator}";
            if (whole > 0 && remainder == 0)
                return whole.ToString();
            // whole == 0
            return $"{remainder}/{denominator}";
        }

        private static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return Math.Abs(a);
        }
    }
}
