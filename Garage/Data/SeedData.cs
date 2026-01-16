using Garage.Constants;
using Garage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Garage.Data;

public class SeedData
{
    private static GarageContext _context = default!;
    private static UserManager<ApplicationUser> _userManager = default!;
    private static RoleManager<IdentityRole> _roleManager = default!;

    public static async Task Initialize(GarageContext context, IServiceProvider services)
    {
        _context = context;
        _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        List<string> defaultRoles = new()
        {
            UserRoles.Member,
            UserRoles.Admin,
        };

        await SeedRoles(defaultRoles);

        var usersToAdd = SeededUsers.Default;

        await SeededUsers.SeedUsers(usersToAdd, _userManager);
        await SeededUsers.AssignRoles(usersToAdd, _userManager);
        await SeededUsers.AddFullNameClaims(usersToAdd, _userManager);
    }

    private static async Task SeedRoles(List<string> roles)
    {
        if (_context.Roles.Any()) return;

        foreach (var role in roles)
        {
            if (await _roleManager.RoleExistsAsync(role))
                continue;

            var result = await _roleManager.CreateAsync(new IdentityRole(role));
            if (!result.Succeeded)
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        }
    }
}
