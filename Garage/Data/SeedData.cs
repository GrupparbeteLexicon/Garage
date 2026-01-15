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

        List<string> defaultRoles = [
            UserRoles.Member,
            UserRoles.Admin,
        ];

        await SeedRoles(defaultRoles);

        var usersToAdd = new[]
            {
                (FirstName: "Admin", LastName: "Adminsson", Email: "admin@Garage.se", PersonalID: "800808-0123", Password: "Admin123!", Role: UserRoles.Admin),
                (FirstName: "Member", LastName: "Membersson", Email: "member@garage.se", PersonalID: "800808-0124", Password: "Member123!", Role: UserRoles.Member)
            };

        var rolesToAssign = usersToAdd.Select(u => (u.Email, u.Role)).ToArray();

        await SeedUsers(usersToAdd);
        await AssignRoles(rolesToAssign);
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

    private static async Task  SeedUsers((string firstName, string lastName, string email, string personalID, string password, string role)[] users)
    {
        foreach (var (firstName, lastName, email, personalID, password, role) in users)
        {
            var userFound = await _userManager.FindByEmailAsync(email);
            if (userFound == null)
            {
                userFound = new ApplicationUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    PersonalID = personalID,
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(userFound, password);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }
    }

    private static async Task AssignRoles((string email, string role)[] userRoles)
    {
        foreach (var (email, role) in userRoles)
        {
            var userFound = await _userManager.FindByEmailAsync(email);
            var isInRole = await _userManager.IsInRoleAsync(userFound, role);

            if (!isInRole)
            {
                var result = await _userManager.AddToRoleAsync(userFound, role);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }
    }
}
