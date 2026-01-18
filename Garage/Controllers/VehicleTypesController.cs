using Garage.Data;
using Garage.Extensions;
using Garage.Models;
using Garage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Garage.Extensions.CountPlacesExtension;

namespace Garage.Controllers
{
    public class VehicleTypesController : Controller
    {
        private readonly GarageContext _context;

        public VehicleTypesController(GarageContext context)
        {
            _context = context;
        }

        // GET: VehicleTypes
        public async Task<IActionResult> Index(string search, VehicleType? type = null)
        {
            var query = _context.VehicleType.AsQueryable();

            var vehicles = await query
                .Select(v => new VehicleTypeViewModel(v))
                .ToListAsync();
            return View(vehicles);
        }

    }
}
