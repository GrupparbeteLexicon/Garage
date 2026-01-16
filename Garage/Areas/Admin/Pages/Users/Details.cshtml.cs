#nullable disable
using Garage.Models;
using Garage.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Garage.Areas.Admin.Pages.Users;

public class DetailsModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    public DetailsModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public string Name { get; set; }
    public List<ParkingVehicleViewModel> Vehicles { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();
        Name = user.UserName ?? string.Empty;
        Vehicles = []; // TODO: Get vehicles by user
        return Page();
    }
}
