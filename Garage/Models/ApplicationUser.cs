using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Garage.Models;

[Index(nameof(PersonalID), IsUnique = true)]
[Index(nameof(FirstName), nameof(LastName))]
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty; // kan inte vara sama som FirstName
    public string FullName => $"{FirstName} {LastName}";
    public string PersonalID { get; set; } = string.Empty;
    public ICollection<Vehicle> OwnedVehicles { get; set; } = new List<Vehicle>();
}
