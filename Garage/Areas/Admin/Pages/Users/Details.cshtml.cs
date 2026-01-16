#nullable disable
using Garage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Garage.Areas.Admin.Pages.Users;

public class DetailsModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    public DetailsModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public string Name { get; set; }
    public List<VehicleModel> Vehicles { get; set; }

    public class VehicleModel
    {
        public string Registration { get; set; }
        public string VehicleType { get; set; }
        public DateTime? ParkTime { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        var user = _userManager.Users
            .Include(s => s.OwnedVehicles)
            .ThenInclude(s => s.VehicleType)
            .Include(s => s.OwnedVehicles)
            .ThenInclude(s => s.ParkingSpot)
            .FirstOrDefault(s => s.Id.Equals(id));
        if (user == null)
            return NotFound();
        Name = user.UserName ?? string.Empty;
        Vehicles = user.OwnedVehicles
            .Select(v => new VehicleModel
            {
                Registration = v.Registration,
                VehicleType = v.VehicleType.Name,
                ParkTime = v.ParkingSpot?.ParkTime,
            })
            .ToList();
        return Page();
    }
}
