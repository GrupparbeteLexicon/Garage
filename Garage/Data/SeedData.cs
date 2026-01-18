using Garage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Garage.Data;

public class SeedData
{
    private static UserManager<ApplicationUser> _userManager = default!;
    private static RoleManager<IdentityRole> _roleManager = default!;

    public static async Task Initialize(GarageContext context, IServiceProvider services)
    {
        _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        var rolesToAdd = SeededUsers.DefaultRoles;
        var usersToAdd = SeededUsers.Default;
        var typesToAdd = SeededVehicleTypes.Default;
        var parksingSpotsToAdd = SeededParkingSpots.Default;

        await SeededUsers.SeedRoles(rolesToAdd, _roleManager);
        await SeededUsers.SeedUsers(usersToAdd, _userManager);
        await SeededUsers.AssignRoles(usersToAdd, _userManager);
        await SeededUsers.AddFullNameClaims(usersToAdd, _userManager);

        await SeededVehicleTypes.SeedVehicleTypes(typesToAdd, context);
        await SeededParkingSpots.SeedParkingSpots(parksingSpotsToAdd, context);
    }
}
