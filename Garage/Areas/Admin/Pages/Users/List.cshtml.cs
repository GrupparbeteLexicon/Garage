#nullable disable
using Garage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Garage.Areas.Admin.Pages.Users;

public class ListModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ListModel(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IList<UserModel> Users { get; set; } = new List<UserModel>();
    public SelectList Roles { get; set; }
    public string Query { get; set; }
    public string RoleFilter { get; set; }

    public class UserModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required IEnumerable<Vehicle> Vehicles { get; set; }
    }

    /// <summary>
    /// List users
    /// </summary>
    /// <param name="q">Query</param>
    /// <param name="role">RoleFilter</param>
    /// <returns></returns>
    public async Task<IActionResult> OnGetAsync(string q, string role)
    {
        var query = _userManager.Users;
        if (role != null)
            query = _userManager.GetUsersInRoleAsync(role).Result.AsQueryable();
        if (q != null)
            query = query.Where(s => s.UserName!.Contains(q));

        var users = query.Include(s => s.OwnedVehicles).ThenInclude(s => s.ParkingSpot).Select(s => new UserModel
        {
            Id = s.Id,
            Name = s.UserName ?? string.Empty,
            Vehicles =  s.OwnedVehicles.ToList(),
        }).ToList();

        var roles = _roleManager.Roles
            .Select(s => new SelectListItem(s.Name, s.Name))
            .ToList();
        Roles = new SelectList(roles, "Value", "Text");
        Users = users;
        Query = q;
        RoleFilter = role;

        return Page();
    }

}
