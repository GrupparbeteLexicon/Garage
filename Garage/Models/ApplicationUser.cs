using Microsoft.AspNetCore.Identity;

namespace Garage.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty; // kan inte vara sama som FirstName
    public string FullName => $"{FirstName} {LastName}";
   public string PersonalID { get; set; } = string.Empty; // måste vara unique

}
