#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Garage.Areas.Admin.Pages.Roles;

[Authorize(Policy = "RequireAdmin")]
public class RoleList : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public RoleList(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public IList<IdentityRole> Roles { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        Roles = await _roleManager.Roles.ToListAsync();
        return Page();
    }
}
