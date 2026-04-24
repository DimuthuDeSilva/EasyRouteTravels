using System.Linq;
using LocalTourPlanner.Models;
using LocalTourPlanner.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LocalTourPlanner.Data; // Ensure this is here for ApplicationDbContext

namespace LocalTourPlanner.Controllers
{
    public class LocationsController : Controller
    {
        private readonly ILocationService _locationService;
        private readonly ApplicationDbContext _context; // Add database context

        public LocationsController(ILocationService locationService, ApplicationDbContext context)
        {
            _locationService = locationService;
            _context = context; // Initialize context
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
                    Distance = x.Distance,
                    ImagePath = x.ImagePath,
                    Feedbacks = x.Feedbacks
                })
                .FirstOrDefault();

            if (location == null)
            {
                return NotFound();
            }

            // --- CHECK IF LOCATION IS IN USER'S PLAN ---
            var userId = HttpContext.Session.GetInt32("UserID");
            bool isLocationInPlan = false;

            if (userId.HasValue)
            {
                // Check database using the integer ID
                isLocationInPlan = _context.TourPlans.Any(tp =>
                    tp.CustomerID == userId.Value &&
                    tp.LocationID == id);
            }

            // Pass this to the View via ViewBag
            ViewBag.IsLocationInPlan = isLocationInPlan;

            return View(location);
        }
    }
}