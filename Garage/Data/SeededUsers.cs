using Garage.Constants;
using Garage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage.Data;

public sealed record SeedUser(ApplicationUser User, string Password, string Role);

public sealed class UserRoles
{
    public const string Admin = "Admin";
    public const string Member = "Member";
}

public static class SeededUsers
{
    public static readonly List<string> DefaultRoles = new()
    {
        UserRoles.Admin,
        UserRoles.Member
    };

    public static readonly IReadOnlyCollection<SeedUser> Default = new[]
    {
        new SeedUser(
            new ApplicationUser
            {
                FirstName = "Admin",
                LastName = "Adminsson",
                PersonalID = "800808-0123",
                UserName = "admin@Garage.se",
                Email = "admin@Garage.se",
                EmailConfirmed = true
            },
            "Admin123!",
            UserRoles.Admin
        ),
        new SeedUser(
            new ApplicationUser
            {
                FirstName = "Member",
                LastName = "Membersson",
                PersonalID = "800808-0124",
                UserName = "member@garage.se",
                Email = "member@garage.se",
                EmailConfirmed = true
            },
            "Member123!",
            UserRoles.Member
        )
    };

    public static async Task SeedRoles(List<string> roles, RoleManager<IdentityRole> roleManager)
    {
        if (roleManager.Roles.Any())  return;

        foreach (var role in roles)
        {
            if (await roleManager.RoleExistsAsync(role))
                continue;

            var result = await roleManager.CreateAsync(new IdentityRole(role));
            if (!result.Succeeded)
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        }
    }

    public static async Task SeedUsers(IEnumerable<SeedUser> users, UserManager<ApplicationUser> userManager)
    {
        foreach (var seed in users)
        {
            var userFound = await userManager.FindByEmailAsync(seed.User.Email);
            if (userFound == null)
            {
                userFound = seed.User;
                var result = await userManager.CreateAsync(userFound, seed.Password);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors.Select(e => e.Description)));
            }
        }
    }

    public static async Task AssignRoles(IEnumerable<SeedUser> userRoles, UserManager<ApplicationUser> userManager)
    {
        foreach (var seed in userRoles)
        {
            var userFound = await userManager.FindByEmailAsync(seed.User.Email);
            if (userFound == null)
                continue;

            var isInRole = await userManager.IsInRoleAsync(userFound, seed.Role);
            if (!isInRole)
            {
                var result = await userManager.AddToRoleAsync(userFound, seed.Role);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors.Select(e => e.Description)));
            }
        }
    }

    public static async Task AddFullNameClaims(IEnumerable<SeedUser> users, UserManager<ApplicationUser> userManager)
    {
        foreach (var seed in users)
        {
            var userFound = await userManager.FindByEmailAsync(seed.User.Email);
            if (userFound == null)
                continue;

            var fullName = $"{userFound.FirstName} {userFound.LastName}";
            var hasClaim = (await userManager.GetClaimsAsync(userFound))
                .Any(c => c.Type == "FullName" && c.Value == fullName);

            if (!hasClaim)
            {
                var claimResult = await userManager.AddClaimAsync(userFound, new System.Security.Claims.Claim("FullName", fullName));
                if (!claimResult.Succeeded) throw new Exception(string.Join("\n", claimResult.Errors.Select(e => e.Description)));
            }
        }
    }
}