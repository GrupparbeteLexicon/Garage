using Garage.Data;
using Garage.Models;
using Garage.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garage.Constants;

namespace Garage.Controllers
{
    public class VehiclesController(GarageContext context, UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly GarageContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        // GET: Vehicles
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Index()
        {
            var garageContext = _context.Vehicle.Include(v => v.Owner).Include(v => v.ParkingSpot).Include(v => v.VehicleType);
            return View(await garageContext.ToListAsync());
        }
        [Authorize]
        public async Task<IActionResult> OwnedVehicles()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            var garageContext = _context.Vehicle.Where(v => v.Owner == user).Include(v => v.Owner).Include(v => v.ParkingSpot).Include(v => v.VehicleType);
            return View(await garageContext.ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Owner)
                .Include(v => v.ParkingSpot)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        [Authorize]
        public IActionResult Create()
        {
            //ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleType, "Id", "Name");
            return View(new CreateOrEditVehicleViewModel());
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VehicleTypeId,Registration,Color,Brand,Model")] CreateOrEditVehicleViewModel createdVehicleViewModel)
        {
            VehicleType vehicleType = await _context.VehicleType.FindAsync(createdVehicleViewModel.VehicleTypeId);
            ApplicationUser owner = await _userManager.GetUserAsync(User);
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(new Vehicle
                    {
                        OwnerId = owner.Id,
                        Owner = owner,
                        VehicleTypeId = createdVehicleViewModel.VehicleTypeId,
                        VehicleType = vehicleType,
                        Registration = createdVehicleViewModel.Registration.ToUpper(),
                        Color = createdVehicleViewModel.Color,
                        Brand = createdVehicleViewModel.Brand,
                        Model = createdVehicleViewModel.Model,
                    });

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. \nMake sure all fields are correct.");
                    Console.WriteLine(ex.Message);
                    return View(createdVehicleViewModel);
                }

                TempData["SuccessMessage"] = $"Vehicle with Registration Number: {createdVehicleViewModel.Registration.ToUpper()} parked successfully!";
                return RedirectToAction(nameof(Index));
                }
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleType, "Id", "Name", createdVehicleViewModel.VehicleTypeId);
            return View(createdVehicleViewModel);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.Include(v => v.VehicleType).FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            var availableSpots = _context.ParkingSpots.Where(x => x.ParkedVehicleID != null || x.Blocked).ToList();
            ViewData["ParkingSpotId"] = new SelectList(availableSpots, "Id", "Name", vehicle.ParkingSpotId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleType, "Id", "Name", vehicle.VehicleTypeId);
            return View(new CreateOrEditVehicleViewModel(vehicle));
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Park(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            var availableSpots = _context.ParkingSpots.Select(x => (x.ParkedVehicleID != null || x.Blocked || x.ParkingSpotSize >= vehicle.VehicleType.VehicleSize)).ToList();
            ViewData["ParkingSpotId"] = new SelectList(availableSpots, "Id", "Name", vehicle.ParkingSpotId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleType, "Id", "Name", vehicle.VehicleTypeId);
            return View(new CreateOrEditVehicleViewModel(vehicle));
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleTypeId,OwnerId,ParkingSpotId,Registration,Color,Brand,Model")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", vehicle.OwnerId);
            ViewData["ParkingSpotId"] = new SelectList(_context.ParkingSpots, "Id", "Id", vehicle.ParkingSpotId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleType, "Id", "Id", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Owner)
                .Include(v => v.ParkingSpot)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicle.Remove(vehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }
    }
}
