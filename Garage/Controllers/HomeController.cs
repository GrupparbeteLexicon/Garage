using Garage.Data;
using Garage.Models;
using Garage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static Garage.Extensions.CountPlacesExtension;

namespace Garage.Controllers
{
    public class HomeController : Controller
    {
        private readonly GarageContext _context;

        public HomeController(GarageContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var query = _context.ParkedVehicle.AsQueryable();
            HomeViewModel homeViewModel = new HomeViewModel();

            int vehiclesParked = query.Count();
            float placesUsed = CountPlaces(query);
            string placesLeft = ToMixedFraction(homeViewModel.Capacity - placesUsed);

            homeViewModel.VehiclesParked = vehiclesParked;
            homeViewModel.PlacesLeft = placesLeft;
            homeViewModel.GarageIsFull = placesUsed > homeViewModel.Capacity;

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
