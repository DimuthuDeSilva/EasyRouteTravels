using System.Linq;
using LocalTourPlanner.Models;
using LocalTourPlanner.Service;
using Microsoft.AspNetCore.Mvc;

namespace LocalTourPlanner.Controllers
{
    public class LocationsController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var data = await _locationService.GetAllLocationAsync();

            var location = data
                .Where(x => x.LID == id)
                .Select(x => new Location
                {
                    LID = x.LID,
                    LocationName = x.LocationName,
                    LocationDescription = x.LocationDescription,
                    ShortDescription = x.ShortDescription,
                    Category = x.Category,
                    Distance = x.Distance
                })
                .FirstOrDefault();

            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }
    }
}