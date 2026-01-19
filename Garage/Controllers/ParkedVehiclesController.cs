using Garage.Data;
using Garage.Extensions;
using Garage.Models;
using Garage.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Garage.Extensions.CountPlacesExtension;

namespace Garage.Controllers
{
    public class ParkedVehiclesController : Controller
    {
        private readonly GarageContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ParkedVehiclesController(GarageContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ParkedVehicles
        public async Task<IActionResult> Index(string? search, int? vehicleTypeId)
        {
            var query = _context.Vehicle
                .Where(v => v.Owner.Email == User.Identity.Name)
                .Include(v => v.VehicleType)
                .Include(v => v.ParkingSpot)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(v => v.Registration.Contains(search));
            }

            if (vehicleTypeId.HasValue)
            {
                query = query.Where(v => v.VehicleType.Id == vehicleTypeId.Value);
            }

            var vehicles = await query
                .Select(v => new ParkingVehicleViewModel(v))
                .ToListAsync();

            var placesUsed = CountPlacesUsed(query);

            ParkingIndexViewModel model = new ParkingIndexViewModel()
            {
                Capacity = GetCapacity(_context),
                Vehicles = vehicles,
                PlacesUsed = placesUsed,
                PlacesLeft = (GetCapacity(_context) - CountPlacesUsed(query)).ToString(),

                Search = search,
                VehicleTypeId = vehicleTypeId,
                VehicleTypeList = new SelectList(_context.VehicleType, "Id", "Name")
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> OwnedVehicles()
        {
            var user = await _userManager.Users
                .Include(u => u.OwnedVehicles)
                    .ThenInclude(v => v.ParkingSpot)
                .Include(u => u.OwnedVehicles)
                    .ThenInclude(v => v.VehicleType)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            if (user == null)
                return Unauthorized();

            var viewModel = new OwnedVehiclesViewModel(user);

            return View(viewModel);
        }


        // GET: ParkedVehicles/Statistics
        public IActionResult Statistics()
        {
            float count = CountPlacesUsed(_context.Vehicle.Include(s => s.VehicleType).AsQueryable());
            DateTime now = DateTime.Now;
            ParkingStatisticsViewModel model = new ParkingStatisticsViewModel()
            {
                
                Capacity = (int)GetCapacity(_context),
                PlacesUsed = (int)Math.Ceiling(count), // show whole places used
                PlacesLeft = (GetCapacity(_context) - count).ToString(),
                HourlyRate = PriceExtentions.HourlyRate, // TODO: Move to configuration or database
                Currency = PriceExtentions.Currency, // TODO: Move to configuration or database
                TotalParkedTime = _context.Vehicle
                    .Include(s => s.ParkingSpot)
                    .Where(s => s.ParkingSpot != null)
                    .Select(s => now - s.ParkingSpot!.ParkTime)
                    .ToList()
                    .Sum(s => (decimal)((TimeSpan)s).TotalHours),
                TotalRevenue = _context.Vehicle
                    .Include(s => s.ParkingSpot)
                    .Where(s => s.ParkingSpot != null)
                    .Select(s => (now - (DateTime)s.ParkingSpot!.ParkTime).ParkedTimeToPrice())
                    .ToList()
                    .Sum(s => s),
                VehicleTypeCounts = _context.Vehicle
                    .Include(s => s.VehicleType)
                    .GroupBy(v => v.VehicleType)
                    .ToDictionary(g => g.Key.Name, g => g.Count()),
            };
            return View(model);
        }

        // GET: ParkedVehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }

            return View(new ParkingVehicleViewModel(parkedVehicle));
        }

        // GET: ParkedVehicles/Park
        [HttpGet, ActionName("Park")]
        public async Task<IActionResult> Create()
        {
            Vehicle parkedVehicle = new Vehicle();
            //parkedVehicle.ParkingSpot.ParkTime = DateTime.Now;

            var query = _context.Vehicle.AsQueryable();
            float placesUsed = CountPlacesUsed(query);
            bool garageIsFull = placesUsed > GetCapacity(_context);

            CreateOrEditViewModel viewModel = await GenerateCreateOrEditViewModel(parkedVehicle, GetCapacity(_context) - placesUsed);
            viewModel.GarageIsFull = garageIsFull;
            viewModel.DisableEditParkTime = true;

            return View(viewModel);
        }


        // POST: ParkedVehicles/Park
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Park")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VehicleTypeId,Registration,Color,Brand,Model")] Vehicle parkedVehicle)
        {
            bool isUnique = ParkedVehicleIsUnique(parkedVehicle.Registration, null);
            var query = _context.Vehicle.Include(s => s.VehicleType).AsQueryable();
            float placesUsed = CountPlacesUsed(query);
			CreateOrEditViewModel viewModel = await GenerateCreateOrEditViewModel(parkedVehicle, GetCapacity(_context) - placesUsed);

			if (parkedVehicle == null) 
            {
                 return Problem("Entity set 'GarageContext.ParkedVehicle'  is null.");
            }

            if (/*ModelState.IsValid && */isUnique)
            {
                try
                {
                    _context.Add(new Vehicle
                    {
                        OwnerId = parkedVehicle.OwnerId,
                        Owner = parkedVehicle.Owner,
                        VehicleTypeId = parkedVehicle.VehicleTypeId,
                        VehicleType = parkedVehicle.VehicleType = await _context.VehicleType.FirstOrDefaultAsync(vt => vt.Id == parkedVehicle.VehicleTypeId),
                        Registration = parkedVehicle.Registration.ToUpper(),
                        Color = parkedVehicle.Color,
                        Brand = parkedVehicle.Brand,
                        Model = parkedVehicle.Model,
                    });

                    await _context.SaveChangesAsync();
                } catch (DbUpdateException ex)
                {
					ModelState.AddModelError("", "Unable to save changes. \nMake sure all fields are correct.");
					Console.WriteLine(ex.Message);
                    return View(parkedVehicle);
                }

				TempData["SuccessMessage"] = $"Vehicle with Registration Number: {parkedVehicle.Registration.ToUpper()} parked successfully!";
				return RedirectToAction(nameof(Index));
            }
            else if (!isUnique)
            {
                ModelState.AddModelError("ParkedVehicle.Registration", "A vehicle with this registration already exists.");
			}

            return View(viewModel);
        }

        // GET: ParkedVehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var query = _context.Vehicle.Include(s => s.VehicleType).AsQueryable();
            float placesUsed = CountPlacesUsed(query);

            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.Vehicle.FindAsync(id);
            var viewModel = GenerateCreateOrEditViewModel(parkedVehicle, GetCapacity(_context) - placesUsed);
            //viewModel.DisableEditParkTime = true;

			return View(viewModel);
        }

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehicle parkedVehicle)
        {
            bool isUnique = ParkedVehicleIsUnique(parkedVehicle.Registration, parkedVehicle.Id);
            var query = _context.Vehicle.Include(s => s.VehicleType).AsQueryable();
            float placesUsed = CountPlacesUsed(query);

            if (id != parkedVehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid && isUnique)
            {
                try
                {
                    // the ParkTime field is disabled, so parkedVehicle.ParkTime here has DateTime.Now (defualt), not the old correct date
                    // even if we don't bind ParkTime, the date still resets to default...
                    // so we ignore that field on save and make sure that the disabled field doesnt reset to default

                    _context.Vehicle
                        .Where(p => p.Id == parkedVehicle.Id)
                        .ExecuteUpdate(setters => setters
                            .SetProperty(p => p.VehicleType, parkedVehicle.VehicleType)
                            .SetProperty(p => p.Registration, parkedVehicle.Registration.ToUpper())
                            .SetProperty(p => p.Color, parkedVehicle.Color)
                            .SetProperty(p => p.Brand, parkedVehicle.Brand)
                            .SetProperty(p => p.Model, parkedVehicle.Model));

                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    if (!ParkedVehicleExists(parkedVehicle.Id))
                    {
						TempData["ErrorMessage"] = $"Vehicle not found";
						return NotFound();
                    }

                    TempData["ErrorMessage"] = $"Could not edit vehicle.";
                    return RedirectToAction(nameof(Index));
                }

				TempData["SuccessMessage"] = $"Vehicle edited successfully!";
				return RedirectToAction(nameof(Index));
            } else
            {
                if (!isUnique)
                {
                    ModelState.AddModelError("ParkedVehicle.Registration", "A vehicle with this registration already exists.");
                }
            }

            CreateOrEditViewModel viewModel = await GenerateCreateOrEditViewModel(parkedVehicle, GetCapacity(_context) - placesUsed);
            return View(viewModel);
        }

        // GET: ParkedVehicles/Unpark/5
        [HttpGet, ActionName("Unpark")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }

            return View(new ParkingVehicleViewModel(parkedVehicle));
        }

        // POST: ParkedVehicles/Unpark/5
        [HttpPost, ActionName("Unpark")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkedVehicle = await _context.Vehicle                
                .FindAsync(id);
            if (parkedVehicle != null)
            {
                DateTime now = DateTime.Now;
                TimeSpan totalParkedTime = now - (DateTime)parkedVehicle.ParkingSpot.ParkTime;

                var receipt = new ReceiptViewModel 
                {
                    Registration = parkedVehicle.Registration,
                    VehicleType = parkedVehicle.VehicleType,
                    ParkTime = parkedVehicle.ParkingSpot.ParkTime,
                    LeaveTime = now,
                    TotalParkedTime = totalParkedTime,
                    TotalPrice = PriceExtentions.CalculateCost(totalParkedTime),
                    Currency = PriceExtentions.Currency
                };

                _context.Vehicle.Remove(parkedVehicle);

                await _context.SaveChangesAsync();
                return View("Receipt", receipt);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ParkedVehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }

        private bool ParkedVehicleIsUnique(string registration, int? id)
        {
            return !_context.Vehicle
                .Where(e => e.Id != id)
                .Any(e => e.Registration == registration);
        }

        private async Task<CreateOrEditViewModel> GenerateCreateOrEditViewModel(Vehicle parkedVehicle, float placesLeft)
        {
            //var vehicleItemList = GetSelectItemsList(placesLeft);
            var vehicleItemList = _context.VehicleType.ToList();

            var viewModel = new CreateOrEditViewModel
            {
                SelectedVehicleType = parkedVehicle.VehicleType,
                VehicleTypeList = new SelectList(vehicleItemList, "Id", "Name"),
                ParkedVehicle = parkedVehicle
            };
            var user = 
            parkedVehicle.VehicleType = _context.VehicleType.FirstOrDefault(vt => vt.Id == parkedVehicle.VehicleTypeId);
            parkedVehicle.Owner = await _userManager.GetUserAsync(User);
            parkedVehicle.OwnerId = _userManager.GetUserId(User);
            return viewModel;
        }
    }
}
