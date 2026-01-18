using Garage.Data;
using Garage.Extensions;
using Garage.Migrations;
using Garage.Models;
using Garage.ViewModels;
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
        private static UserManager<ApplicationUser> _userManager = default!;

        public ParkedVehiclesController(GarageContext context, IServiceProvider services)
        {
            _context = context;
            _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        }

        // GET: ParkedVehicles
        public async Task<IActionResult> Index(string? search, int? vehicleTypeId)
        {
            var query = _context.ParkedVehicle
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

            var placesUsed = CountPlaces(query);

            ParkingIndexViewModel model = new ParkingIndexViewModel()
            {
                Capacity = GetCapacity(_context),
                Vehicles = vehicles,
                PlacesUsed = placesUsed,
                PlacesLeft = ToMixedFraction(GetCapacity(_context) - CountPlaces(query)),
                Search = search,
                VehicleTypeId = vehicleTypeId,
                VehicleTypeList = new SelectList(_context.VehicleType, "Id", "Name")
            };

            return View(model);
        }

        // GET: ParkedVehicles/Statistics
        public IActionResult Statistics()
        {
            float count = CountPlaces(_context.ParkedVehicle.AsQueryable());
            DateTime now = DateTime.Now;
            ParkingStatisticsViewModel model = new ParkingStatisticsViewModel()
            {
                
                Capacity = (int)GetCapacity(_context),
                PlacesUsed = (int)Math.Ceiling(count), // show whole places used
                PlacesLeft = ToMixedFraction(GetCapacity(_context) - count),
                HourlyRate = PriceExtentions.HourlyRate, // TODO: Move to configuration or database
                Currency = PriceExtentions.Currency, // TODO: Move to configuration or database
                TotalParkedTime = _context.ParkedVehicle
                    .Select(s => now - s.ParkingSpot.ParkTime)
                    .ToList()
                    .Sum(s => (decimal)s.TotalHours),
                TotalRevenue = _context.ParkedVehicle
                    .Select(s => (now - s.ParkingSpot.ParkTime).ParkedTimeToPrice())
                    .ToList()
                    .Sum(s => s),
                VehicleTypeCounts = _context.ParkedVehicle
                    .GroupBy(v => v.VehicleType)
                    .ToDictionary(g => g.Key.Name, g => g.Count()),
            };
            return View(model);
        }

        // GET: ParkedVehicles/Manage
        public async Task<IActionResult> Manage(string? search, int? vehicleTypeId)
        {
            var query = _context.ParkedVehicle
                .Include(v => v.Owner)
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
                .Select(v => new ManageVehicleViewModel(v))
                .ToListAsync();

            var viewModel = new ManageViewModel
            {
                Vehicles = vehicles,
                VehicleTypeList = new SelectList(_context.VehicleType, "Id", "Name"),
                VehicleTypeId = vehicleTypeId,
                Search = search
            };

            return View(viewModel);
        }

        // GET: ParkedVehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle
                .Include(v => v.VehicleType)
                .Include(v => v.ParkingSpot)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }

            return View(new ParkingVehicleViewModel(parkedVehicle));
        }

        // GET: ParkedVehicles/Park
        [HttpGet, ActionName("Park")]
        public IActionResult Create()
        {
            SelectList vehicleTypes = new SelectList(_context.VehicleType, "Id", "Name");

            var query = _context.ParkedVehicle.AsQueryable();
            float placesUsed = CountPlaces(query);
            float capacity = GetCapacity(_context);

            var viewModel = new CreateOrEditViewModel
            {
                VehicleTypeList = vehicleTypes,
                GarageIsFull = placesUsed > capacity
            };

            return View(viewModel);
        }

       // POST: ParkedVehicles/Park
       [HttpPost, ActionName("Park")]
       [ValidateAntiForgeryToken]
        public IActionResult Create(CreateOrEditViewModel input)
        {
            Console.WriteLine(">>> POST Park HIT <<<");

            if (!ModelState.IsValid)
            {
                input.VehicleTypeList = new SelectList(_context.VehicleType, "Id", "Name");
                return View(input);
            }

            var vehicleType = _context.VehicleType.Find(input.VehicleTypeId);

            // Find current logged-in user
            var owner = _userManager.GetUserAsync(User).Result;

            // Find empty parking space
            var parkingSpot = _context.ParkingSpots
                .Where(ps => ps.ParkedVehicleID == null)
                .FirstOrDefault();

            var vehicle = new Vehicle
            {
                Registration = input.Registration,
                Color = input.Color,
                Brand = input.Brand,
                Model = input.Model,
                VehicleTypeId = input.VehicleTypeId,
                VehicleType = vehicleType,
                OwnerId = owner.Id,
                Owner = owner,
                //ParkingSpotId = parkingSpot.Id,
                ParkingSpot = parkingSpot
            };

            if (parkingSpot == null)
            {
                ModelState.AddModelError("", "No available parking spots.");
                input.VehicleTypeList = new SelectList(_context.VehicleType, "Id", "Name");
                return View(input);
            }

            parkingSpot.ParkedVehicle = vehicle;
            parkingSpot.ParkTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.ParkedVehicle.Add(vehicle);
                    _context.SaveChanges();

                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. \nMake sure all fields are correct.");
                    Console.WriteLine(ex.Message);
                    return View(input);
                }
            }

            return RedirectToAction("Index");
        }

        //     // POST: ParkedVehicles/Park
        //     // To protect from overposting attacks, enable the specific properties you want to bind to.
        //     // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //     [HttpPost, ActionName("Park")]
        //     [ValidateAntiForgeryToken]
        //     public async Task<IActionResult> Create([Bind("Id,VehicleTypeId,Registration,Color,Brand,Model")] Vehicle parkedVehicle)
        //     //public async Task<IActionResult> Create(Vehicle parkedVehicle)
        //     {
        //         bool isUnique = ParkedVehicleIsUnique(parkedVehicle.Registration, null);
        //         var query = _context.ParkedVehicle.AsQueryable();
        //         float placesUsed = CountPlaces(query);
        //         CreateOrEditViewModel viewModel = GenerateCreateOrEditViewModel(parkedVehicle, GetCapacity(_context) - placesUsed);

        //         if (parkedVehicle == null)
        //         {
        //             return Problem("Entity set 'GarageContext.ParkedVehicle'  is null.");
        //         }

        //         if (ModelState.IsValid && isUnique)
        //         {
        //             try
        //             {
        //                 _context.Add(new Vehicle
        //                 {
        //                     OwnerId = parkedVehicle.OwnerId,
        //                     Owner = parkedVehicle.Owner,
        //                     VehicleTypeId = parkedVehicle.VehicleTypeId,
        //                     VehicleType = parkedVehicle.VehicleType,
        //                     Registration = parkedVehicle.Registration.ToUpper(),
        //                     Color = parkedVehicle.Color,
        //                     Brand = parkedVehicle.Brand,
        //                     Model = parkedVehicle.Model,
        //                 });

        //                 await _context.SaveChangesAsync();
        //             }
        //             catch (DbUpdateException ex)
        //             {
        //                 ModelState.AddModelError("", "Unable to save changes. \nMake sure all fields are correct.");
        //                 Console.WriteLine(ex.Message);
        //                 return View(parkedVehicle);
        //             }

        //             TempData["SuccessMessage"] = $"Vehicle with Registration Number: {parkedVehicle.Registration.ToUpper()} parked successfully!";
        //             return RedirectToAction(nameof(Index));
        //         }
        //         else if (!isUnique)
        //         {
        //             ModelState.AddModelError("ParkedVehicle.Registration", "A vehicle with this registration already exists.");
        //         }

        //         return View(viewModel);
        //     }

        // GET: ParkedVehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) { return NotFound(); }

            var query = _context.ParkedVehicle.AsQueryable();
            float placesUsed = CountPlaces(query);

            var vehicle = await _context.ParkedVehicle
                .Where(v => v.OwnerId == _userManager.GetUserAsync(User).Result!.Id)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vehicle == null) { return NotFound(); }
            float capacity = GetCapacity(_context);

            var viewModel = GenerateEditViewModel(vehicle, GetCapacity(_context) - placesUsed);

            return View(viewModel);
        }

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehicle parkedVehicle)
        {
            //bool isUnique = ParkedVehicleIsUnique(parkedVehicle.Registration, parkedVehicle.Id);
            var query = _context.ParkedVehicle.AsQueryable();
            float placesUsed = CountPlaces(query);

            if (id != parkedVehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // the ParkTime field is disabled, so parkedVehicle.ParkTime here has DateTime.Now (defualt), not the old correct date
                    // even if we don't bind ParkTime, the date still resets to default...
                    // so we ignore that field on save and make sure that the disabled field doesnt reset to default

                    _context.ParkedVehicle
                        .Where(p => p.Id == parkedVehicle.Id)
                        .Include(p => p.VehicleType)
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
            } 
            //else
            //{
            //    if (!isUnique)
            //    {
            //        ModelState.AddModelError("ParkedVehicle.Registration", "A vehicle with this registration already exists.");
            //    }
            //}

            CreateOrEditViewModel viewModel = GenerateEditViewModel(parkedVehicle, GetCapacity(_context) - placesUsed);
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

            var parkedVehicle = await _context.ParkedVehicle
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
            var parkedVehicle = await _context.ParkedVehicle.FindAsync(id);
            if (parkedVehicle != null)
            {
                DateTime now = DateTime.Now;
                TimeSpan totalParkedTime = now - parkedVehicle.ParkingSpot.ParkTime;

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

                _context.ParkedVehicle.Remove(parkedVehicle);

                await _context.SaveChangesAsync();
                return View("Receipt", receipt);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ParkedVehicleExists(int id)
        {
            return _context.ParkedVehicle.Any(e => e.Id == id);
        }

        //private bool ParkedVehicleIsUnique(string registration, int? id)
        //{
        //    return !_context.ParkedVehicle
        //        .Where(e => e.Id != id)
        //        .Any(e => e.Registration == registration);
        //}

        private CreateOrEditViewModel GenerateEditViewModel(Vehicle parkedVehicle, float placesLeft)
        {
            var vehicleItemList = GetSelectItemsList(placesLeft);
            SelectList vehicleTypes = new SelectList(_context.VehicleType, "Id", "Name");

            var viewModel = new CreateOrEditViewModel
            {
                Registration = parkedVehicle.Registration,
                Color = parkedVehicle.Color,
                Brand = parkedVehicle.Brand,
                Model = parkedVehicle.Model,
                VehicleTypeId = parkedVehicle.VehicleTypeId,

                SelectedVehicleType = parkedVehicle.VehicleType,
                VehicleTypeList = vehicleTypes
            };

            return viewModel;
        }
    }
}