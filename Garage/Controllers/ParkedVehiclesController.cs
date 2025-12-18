using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage.Data;
using Garage.Models;
using Garage.ViewModels;

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
        public async Task<IActionResult> Index(string search)
        {
            var query = _context.ParkedVehicle.AsQueryable();

            ViewData["Search"] = search;

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(v => v.Registration.Contains(search));
            }

            var vehicles = await query.ToListAsync();
            return View(vehicles);
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

            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Park
        [HttpGet, ActionName("Park")]
        public IActionResult Create()
        {
            ParkedVehicle parkedVehicle = new ParkedVehicle();
            parkedVehicle.ParkTime = DateTime.Now;

            CreateOrEditViewModel viewModel = GenerateCreateOrEditViewModel(parkedVehicle);

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

            if (parkedVehicle == null) 
            {
                 return Problem("Entity set 'GarageContext.ParkedVehicle'  is null.");
            }

            if (ModelState.IsValid && isUnique)
            {
                _context.Add(parkedVehicle);
                try
                {
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

            CreateOrEditViewModel viewModel = GenerateCreateOrEditViewModel(parkedVehicle);
            return View(viewModel);
        }

        // GET: ParkedVehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle.FindAsync(id);
            var viewModel = GenerateCreateOrEditViewModel(parkedVehicle);

            return View(viewModel);
        }

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleType,Registration,Color,Brand,Model,Wheels,ParkTime")] ParkedVehicle parkedVehicle)
        {
           if (id != parkedVehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkedVehicle);
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

            CreateOrEditViewModel viewModel = GenerateCreateOrEditViewModel(parkedVehicle);
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

            return View(parkedVehicle);
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

        private CreateOrEditViewModel GenerateCreateOrEditViewModel(ParkedVehicle parkedVehicle)
        {
            var vehicleTypeList = Enum.GetValues(typeof(VehicleType))
               .Cast<VehicleType>()
               .Select(type => new SelectListItem
               {
                   Value = ((int)type).ToString(),
                   Text = type.ToString()
               })
               .ToList();

            var viewModel = new CreateOrEditViewModel
            {
                SelectedVehicleType = parkedVehicle.VehicleType,
                VehicleTypeList = new SelectList(vehicleTypeList, "Value", "Text"),
                ParkedVehicle = parkedVehicle
            };

            return viewModel;
        }
    }
}
