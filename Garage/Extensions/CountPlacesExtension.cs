using Garage.Data;
using Garage.Models;
using Garage.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Garage.Extensions;

public static class CountPlacesExtension
{
    public static float GetCapacity(GarageContext context)
    {
        return context.ParkingSpots.Count();
    }

    public static float CountPlacesUsed(IQueryable<Vehicle> vehicles)
    {
        int parkedVehiclesCount = vehicles.Where(v => v.ParkingSpot != null).ToList().Count();

        return parkedVehiclesCount;
    }
}