using Garage.Data;
using Garage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Garage.Areas.Admin.Pages.Users;

public class ListModel : PageModel
{
    private readonly GarageContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ListModel(GarageContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IList<UserModel> Users { get; set; } = new List<UserModel>();

    public class UserModel
    {
        public string Id { get; set; }
        public required string Name { get; set; }
        public required IEnumerable<string> Roles { get; set; }
        public int VehicleCount { get; set; }
        public int TotalRevenue { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            Users.Add(new UserModel
            {
                Id = user.Id,
                Name = user.UserName ?? string.Empty,
                Roles = roles,
                VehicleCount = 0, // TODO: Add count from context
                TotalRevenue = 0, // TODO: Add revenue from context
            });
        }

        return Page();
    }
}
