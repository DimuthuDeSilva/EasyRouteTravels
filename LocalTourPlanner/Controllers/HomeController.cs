using System.Linq;
using LocalTourPlanner.Data;
using LocalTourPlanner.Models;
using LocalTourPlanner.Service;
using Microsoft.AspNetCore.Mvc;

namespace LocalTourPlanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILocationService _locationService;

        public HomeController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _locationService.GetAllLocationAsync();

            var model = new LocationViewModel
            {
                Locations = data.Select(x => new Location
                {
                    LID = x.LID,
                    LocationName = x.LocationName,
                    LocationDescription = x.LocationDescription,
                    ShortDescription = x.ShortDescription,
                    Category = x.Category,
                    Distance = x.Distance,
                }).ToList()
            };

            return View(model);
        }
    }
}