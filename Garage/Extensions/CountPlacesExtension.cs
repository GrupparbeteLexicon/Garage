using Garage.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Garage.Extensions;

public static class CountPlacesExtension
{
    public static float Capacity { get; } = 50f;

    public static float CountPlaces(IQueryable<ParkedVehicle> vehicles)
    {
        int placesUsed = 0;

        foreach (var vehicle in vehicles)
        {
            placesUsed += GetPlaceSizeForVehicleType(vehicle.VehicleType);
        }

        return (float)placesUsed / 3; // Since 1 Place = 3 Units
    }

    public static float CountPlaces(IEnumerable<Garage.ViewModels.ParkingVehicleViewModel> vehicles)
    {
        int placesUsed = 0;

        foreach (var vehicle in vehicles)
        {
            placesUsed += GetPlaceSizeForVehicleType(vehicle.VehicleType);
        }

        return (float)placesUsed / 3; // Since 1 Place = 3 Units
    }

    private static int GetPlaceSizeForVehicleType(VehicleType vehicleType)
    {
        return vehicleType switch
        {
            VehicleType.MOTORCYCLE => 1, // 1/3 Place
            VehicleType.ATV => 3, // 1 Place
            VehicleType.CAR => 3, // 1 Place
            VehicleType.AIRPLANE => 9, // 3 Places
            VehicleType.BOAT => 9, // 3 Places
            VehicleType.BUS => 9, // 3 Places
            VehicleType.TRUCK => 6, // 2 Places
            _ => 1
        };
    }

    public static List<SelectListItem> GetSelectItemsList(float placesLeft)
    {
        var vehicleTypeList = new List<SelectListItem>();

        if (placesLeft > 0.2f)
        {
            vehicleTypeList.Add(new SelectListItem
            {
                Value = VehicleType.MOTORCYCLE.ToString(),
                Text = VehicleType.MOTORCYCLE.GetDisplayName()
            });
        }

        if (placesLeft >= 1)
        {
            vehicleTypeList.Add(new SelectListItem
            {
                Value = VehicleType.ATV.ToString(),
                Text = VehicleType.ATV.GetDisplayName()
            });

            vehicleTypeList.Add(new SelectListItem
            {
                Value = VehicleType.CAR.ToString(),
                Text = VehicleType.CAR.GetDisplayName()
            });
        }

        if (placesLeft >= 2)
        {
            vehicleTypeList.Add(new SelectListItem
            {
                Value = VehicleType.TRUCK.ToString(),
                Text = VehicleType.TRUCK.GetDisplayName()
            });
        }

        if (placesLeft >= 3)
        {
            vehicleTypeList.Add(new SelectListItem
            {
                Value = VehicleType.AIRPLANE.ToString(),
                Text = VehicleType.AIRPLANE.GetDisplayName()
            });

            vehicleTypeList.Add(new SelectListItem
            {
                Value = VehicleType.BOAT.ToString(),
                Text = VehicleType.BOAT.GetDisplayName()
            });

            vehicleTypeList.Add(new SelectListItem
            {
                Value = VehicleType.BUS.ToString(),
                Text = VehicleType.BUS.GetDisplayName()
            });
        }

        return vehicleTypeList;
    }

    public static string GetDisplayName(this Enum value)
    {
        var member = value.GetType()
                          .GetMember(value.ToString())
                          .FirstOrDefault();

        if (member == null)
            return value.ToString();

        var display = member.GetCustomAttribute<DisplayAttribute>();
        return display?.Name ?? value.ToString();
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
