using Garage.Constants;
using Garage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Garage.Data;

public class SeedData
{
    private static UserManager<ApplicationUser> _userManager = default!;
    private static RoleManager<IdentityRole> _roleManager = default!;

    public static async Task Initialize(GarageContext context, IServiceProvider services)
    {
        _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        if (context.Roles.Any())
            return;

        List<string> defaultRoles = new()
        {
            UserRoles.Member,
            UserRoles.Admin,
        };

        await SeedRoles(defaultRoles);

        var usersToAdd = SeededUsers.Default;

        await SeedUsers(usersToAdd);
        await AssignRoles(usersToAdd);
        await AddFullNameClaims(usersToAdd);
    }

    private static async Task SeedRoles(List<string> roles)
    {
        foreach (var role in roles)
        {
            if (await _roleManager.RoleExistsAsync(role))
                continue;

            var result = await _roleManager.CreateAsync(new IdentityRole(role));
            if (!result.Succeeded)
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        }
    }

    private static async Task SeedUsers(IEnumerable<SeedUser> users)
    {
        foreach (var seed in users)
        {
            var userFound = await _userManager.FindByEmailAsync(seed.User.Email);
            if (userFound == null)
            {
                userFound = seed.User;

                var result = await _userManager.CreateAsync(userFound, seed.Password);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }
    }

    private static async Task AssignRoles(IEnumerable<SeedUser> userRoles)
    {
        foreach (var seed in userRoles)
        {
            var userFound = await _userManager.FindByEmailAsync(seed.User.Email);
            var isInRole = await _userManager.IsInRoleAsync(userFound, seed.Role);

            if (!isInRole)
            {
                var result = await _userManager.AddToRoleAsync(userFound, seed.Role);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }
    }

    private static async Task AddFullNameClaims(IEnumerable<SeedUser> users)
    {
        foreach (var seed in users)
        {
            var userFound = await _userManager.FindByEmailAsync(seed.User.Email);
            var fullName = $"{userFound.FirstName} {userFound.LastName}";
            var hasClaim = (await _userManager.GetClaimsAsync(userFound))
                .Any(c => c.Type == "FullName" && c.Value == fullName);
            if (!hasClaim)
            {
                var claimResult = await _userManager.AddClaimAsync(userFound, new System.Security.Claims.Claim("FullName", fullName));
                if (!claimResult.Succeeded) throw new Exception(string.Join("\n", claimResult.Errors));
            }
        }
    }
}
