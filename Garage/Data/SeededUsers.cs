using Garage.Constants;
using Garage.Models;
using System.Collections.Generic;

namespace Garage.Data;

public sealed record SeedUser(ApplicationUser User, string Password, string Role);

public static class SeededUsers
{
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
}