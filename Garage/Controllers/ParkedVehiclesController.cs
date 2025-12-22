using Garage.Data;
using Garage.Extensions;
using Garage.Models;
using Garage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Garage.Extensions.CountPlacesExtension;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Garage.Controllers
{
    public class ParkedVehiclesController : Controller
    {
        private readonly GarageContext _context;

        public ParkedVehiclesController(GarageContext context)
        {
            _context = context;
        }

        // GET: ParkedVehicles
        public async Task<IActionResult> Index(string search, VehicleType? type = null)
        {
            var query = _context.ParkedVehicle.AsQueryable();

            ViewData["Search"] = search;

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(v => v.Registration.Contains(search));
            }
            if (type != null)
            {
                query = query.Where(v => v.VehicleType == type);
            }

            var vehicles = await query
                .Select(v => new ParkingVehicleViewModel(v))
                .ToListAsync();
            return View(vehicles);
        }

        // GET: ParkedVehicles/Statistics
        public IActionResult Statistics()
        {
            float count = CountPlaces(_context.ParkedVehicle.AsQueryable());
            ParkingStatisticsViewModel model = new ParkingStatisticsViewModel()
            {
                Capacity = (int)CountPlacesExtension.Capacity,
                PlacesUsed = (int)Math.Ceiling(count), // show whole places used
                PlacesLeft = ToMixedFraction(CountPlacesExtension.Capacity - count),
                HourlyRate = 20.0M, // TODO: Move to configuration or database
                Currency = "kr", // TODO: Move to configuration or database
                TotalParkedTime = _context.ParkedVehicle
                    .Select(s => DateTime.Now - s.ParkTime)
                    .ToList()
                    .Sum(s => (decimal)s.TotalHours),
                VehicleTypeCounts = _context.ParkedVehicle
                    .GroupBy(v => v.VehicleType)
                    .ToDictionary(g => g.Key.GetDisplayName(), g => g.Count()),
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

            var parkedVehicle = await _context.ParkedVehicle
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
            ParkedVehicle parkedVehicle = new ParkedVehicle();
            parkedVehicle.ParkTime = DateTime.Now;

            var query = _context.ParkedVehicle.AsQueryable();
            float placesUsed = CountPlaces(query);
            bool garageIsFull = placesUsed > Capacity;

            CreateOrEditViewModel viewModel = GenerateCreateOrEditViewModel(parkedVehicle, Capacity - placesUsed);
            viewModel.GarageIsFull = garageIsFull;
            viewModel.DisableEditParkTime = true;

            return View(viewModel);
        }

        // POST: ParkedVehicles/Park
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Park")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VehicleType,Registration,Color,Brand,Model,Wheels,ParkTime")] ParkedVehicle parkedVehicle)
        {
            bool isUnique = ParkedVehicleIsUnique(parkedVehicle.Registration);
            var query = _context.ParkedVehicle.AsQueryable();
            float placesUsed = CountPlaces(query);

            if (parkedVehicle == null) 
            {
                 return Problem("Entity set 'GarageContext.ParkedVehicle'  is null.");
            }

            if (ModelState.IsValid && isUnique)
            {
               try
                {
                    _context.Add(new ParkedVehicle
                    {
                        VehicleType = parkedVehicle.VehicleType,
                        Registration = parkedVehicle.Registration.ToUpper(),
                        Color = parkedVehicle.Color,
                        Brand = parkedVehicle.Brand,
                        Model = parkedVehicle.Model,
                        Wheels = parkedVehicle.Wheels,
                        ParkTime = DateTime.Now
                    });

                    await _context.SaveChangesAsync();
                } catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. \nMake sure all fields are correct.");
                    Console.WriteLine(ex.Message);
                    return View(parkedVehicle);
                }
                return RedirectToAction(nameof(Index));
            }
            else if (!isUnique)
            {
                ModelState.AddModelError("ParkedVehicle.Registration", "A vehicle with this registration already exists.");
            }

            CreateOrEditViewModel viewModel = GenerateCreateOrEditViewModel(parkedVehicle, Capacity - placesUsed);
            return View(viewModel);
        }

        // GET: ParkedVehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var query = _context.ParkedVehicle.AsQueryable();
            float placesUsed = CountPlaces(query);

            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle.FindAsync(id);
            var viewModel = GenerateCreateOrEditViewModel(parkedVehicle, Capacity - placesUsed);
            viewModel.DisableEditParkTime = true;

            return View(viewModel);
        }

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ParkedVehicle parkedVehicle)
        {
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
                        .ExecuteUpdate(setters => setters
                            .SetProperty(p => p.VehicleType, parkedVehicle.VehicleType)
                            .SetProperty(p => p.Registration, parkedVehicle.Registration.ToUpper())
                            .SetProperty(p => p.Color, parkedVehicle.Color)
                            .SetProperty(p => p.Brand, parkedVehicle.Brand)
                            .SetProperty(p => p.Model, parkedVehicle.Model)
                            .SetProperty(p => p.Wheels, parkedVehicle.Wheels));

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkedVehicleExists(parkedVehicle.Id))
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

            CreateOrEditViewModel viewModel = GenerateCreateOrEditViewModel(parkedVehicle, Capacity - placesUsed);
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
                _context.ParkedVehicle.Remove(parkedVehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkedVehicleExists(int id)
        {
            return _context.ParkedVehicle.Any(e => e.Id == id);
        }

        private bool ParkedVehicleIsUnique(string registration)
        {
            return !_context.ParkedVehicle.Any(e => e.Registration == registration);
        }

        private CreateOrEditViewModel GenerateCreateOrEditViewModel(ParkedVehicle parkedVehicle, float placesLeft)
        {
            var vehicleItemList = GetSelectItemsList(placesLeft);

            var viewModel = new CreateOrEditViewModel
            {
                SelectedVehicleType = parkedVehicle.VehicleType,
                VehicleTypeList = new SelectList(vehicleItemList, "Value", "Text"),
                ParkedVehicle = parkedVehicle
            };

            return viewModel;
        }
    }
}
