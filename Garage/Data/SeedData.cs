using Bogus;
using Garage.Constants;
using Garage.Models;
using Microsoft.AspNetCore.Identity;
using Vehicle = Garage.Models.Vehicle;

namespace Garage.Data;

public class SeedData
{
    private static readonly Faker Faker = new("sv");
    private static GarageContext _context = default!;
    private static UserManager<ApplicationUser> _userManager = default!;
    private static RoleManager<IdentityRole> _roleManager = default!;

    public static async Task Initialize(GarageContext context, IServiceProvider services)
    {
        _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        _context = context;

        if (!context.Roles.Any())
            await SeedRoles();

        if (!context.Users.Any())
        {
            var usersToAdd = new[]
            {
                (FirstName: "Admin", LastName: "Adminsson", Email: "admin@Garage.se", PersonalID: "800808-0123",
                    Password: "Admin123!", Role: UserRoles.Admin),
                (FirstName: "Member", LastName: "Membersson", Email: "member@garage.se", PersonalID: "800808-0124",
                    Password: "Member123!", Role: UserRoles.Member)
            };

            var rolesToAssign = usersToAdd.Select(u => (u.Email, u.Role)).ToArray();

            await SeedRandomUsers(100);
            await SeedUsers(usersToAdd);
            await AssignRoles(rolesToAssign);
        }
        if (!context.VehicleType.Any())
            await SeedVehicleTypes();
        if (!context.ParkingSpots.Any())
            await SeedParkingSpots(200);
        if (!context.Vehicle.Any())
        {
            foreach (var user in _userManager.Users)
                await SeedUserVehicles(user.Id, Faker.Random.Int(0, 12));
        }
        await _context.SaveChangesAsync();
    }

    private static async Task SeedRandomUsers(int count)
    {
        var faker = new Faker<ApplicationUser>()
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
            .RuleFor(u => u.UserName, (_, u) => u.Email)
            .RuleFor(u => u.EmailConfirmed, true)
            .RuleFor(u => u.PersonalID, f => f.Random.Replace(f.Date.Past(80).ToString("yyMMdd-####")));
        var users = faker.Generate(count);
        foreach (var user in users)
        {
            var result = await _userManager.CreateAsync(user, "Abc123!");
            if (!result.Succeeded)
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        }
        await AssignRoles(users.Select(s => (s.Email!, UserRoles.Member)).ToArray());
    }

    private static async Task SeedRoles()
    {
        List<string> defaultRoles =
        [
            UserRoles.Member,
            UserRoles.Admin,
        ];

        foreach (var role in defaultRoles)
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

    private static async Task SeedVehicleTypes()
    {
        VehicleType[] types = [
            new() {Name = "Car",        VehicleSize = 1},
            new() {Name = "Van",        VehicleSize = 1},
            new() {Name = "Motorcycle", VehicleSize = 0},
            new() {Name = "Truck",      VehicleSize = 2},
            new() {Name = "ATV",        VehicleSize = 0},
            new() {Name = "Bus",        VehicleSize = 2},
        ];

        await _context.VehicleType.AddRangeAsync(types);
        await _context.SaveChangesAsync();
    }

    private static async Task SeedParkingSpots(int count)
    {
        var vehicleSizes = _context.VehicleType.Select(s => s.VehicleSize).ToArray();
        var faker = new Faker<ParkingSpot>()
       .RuleFor(v => v.ParkingSpotSize, f => f.PickRandom(vehicleSizes))
       .RuleFor(v => v.Name, f =>
       {
           var index = f.UniqueIndex;
           char section = (char)('A' + (index / 50) % 5); // A–E
           int number = (index % 50) + 1;

           return $"{section}-{number:D2}";
       });
        var parkingSpots = faker.Generate(count);
        await _context.ParkingSpots.AddRangeAsync(parkingSpots);
    }

    private static async Task SeedUserVehicles(string userId, int count)
    {
        var types = _context.VehicleType.Select(s => s.Id).ToArray();
        var faker = new Faker<Vehicle>()
            .RuleFor(v => v.Registration, f => f.Random.Replace("???##*").ToUpper())
            .RuleFor(v => v.OwnerId, userId)
            .RuleFor(v => v.VehicleTypeId, f => f.PickRandom(types))
            .RuleFor(v => v.Brand, f => f.Vehicle.Manufacturer())
            .RuleFor(v => v.Model, f => f.Vehicle.Model())
            .RuleFor(v => v.Color, f => f.Commerce.Color());
        var vehicles = faker.Generate(count);
        await _context.Vehicle.AddRangeAsync(vehicles);
    }
}
