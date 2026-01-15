using Garage.Constants;
using Microsoft.AspNetCore.Identity;

namespace Garage.Data;

public class SeedData
{
    public static async Task Initialize(GarageContext context, IServiceProvider services)
    {
        if (context.Roles.Any())
            return;
        List<string> defaultRoles = [
            UserRoles.Member,
            UserRoles.Admin,
        ];

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await SeedRoles(roleManager, defaultRoles);
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> manager, List<string> roles)
    {
        foreach (var role in roles)
        {
            if (await manager.RoleExistsAsync(role))
                continue;
            var result = await manager.CreateAsync(new IdentityRole(role));
            if (!result.Succeeded)
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        }
    }
}
