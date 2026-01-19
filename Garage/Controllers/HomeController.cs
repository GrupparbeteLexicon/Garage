using Garage.Data;
using Garage.Extensions;
using Garage.Models;
using Garage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static Garage.Extensions.CountPlacesExtension;

namespace Garage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GarageContext _context;

        public HomeController(ILogger<HomeController> logger, GarageContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var query = _context.Vehicle.AsQueryable();
            HomeViewModel homeViewModel = new HomeViewModel();

            float capacity = GetCapacity(_context);
            float placesUsed = CountPlacesUsed(query);
            string placesLeft = (capacity - placesUsed).ToString();

            homeViewModel.Capacity = capacity;
            homeViewModel.VehiclesParked = (int)placesUsed;
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
