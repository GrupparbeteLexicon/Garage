using Garage.Data;
using Garage.Models;
using static Garage.Extensions.CountPlacesExtension;
using Garage.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Garage.Controllers
{
    public class KvittoController : Controller
    {
        
        // GET: /Kvitto
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
