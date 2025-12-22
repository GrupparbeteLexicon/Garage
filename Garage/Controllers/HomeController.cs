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
        private readonly ILogger<HomeController> _logger;
        private readonly GarageContext _context;

        public HomeController(ILogger<HomeController> logger, GarageContext context)
        {
            _logger = logger;
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

        public IActionResult Statistics()
        {
            ParkingStatisticsViewModel model = new ParkingStatisticsViewModel()
            {
                Capacity = (int)Capacity,
                Count = _context.ParkedVehicle.Count(),
                HourlyRate = 20.0M, // TODO: Move to configuration or database
                Currency = "kr", // TODO: Move to configuration or database
                TotalParkedTime = _context.ParkedVehicle
                    .Select(s => DateTime.Now - s.ParkTime)
                    .ToList()
                    .Sum(s => (decimal)s.TotalHours),
                VehicleTypeCounts = _context.ParkedVehicle
                    .GroupBy(v => v.VehicleType)
                    .ToDictionary(g => g.Key, g => g.Count()),
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
