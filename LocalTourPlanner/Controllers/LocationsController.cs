using LocalTourPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using LocalTourPlanner.Service.Interfaces; 

namespace LocalTourPlanner.Controllers
{
    public class LocationsController : Controller
    {
        #region Fields
        private readonly ILocationService _locationService;
        private readonly ITourPlanService _tourPlanService;
        #endregion

        #region Ctor
        public LocationsController(ILocationService locationService, ITourPlanService tourPlanService)
        {
            _locationService = locationService;
            _tourPlanService = tourPlanService;
        }
        #endregion

        #region Methods
        public async Task<IActionResult> Details(int id)
        {
            var data = await _locationService.GetAllLocationAsync();

            var location = data
                .Where(x => x.LID == id)
                .Select(x => new LocationModel
                {
                    LID = x.LID,
                    LocationName = x.LocationName,
                    LocationDescription = x.LocationDescription,
                    ShortDescription = x.ShortDescription,
                    Category = x.Category,
                    Distance = x.Distance,
                    ImagePath = x.ImagePath,
                    
                    Feedbacks = x.Feedbacks != null
                ? x.Feedbacks.Select(f => new FeedbackModel
                {
                    FeedbackID = f.FeedbackID,
                    LocationID = f.LocationID,
                    UserName = f.UserName,
                    Comment = f.Comment,
                    Rating = f.Rating,
                    ImagePath = f.ImagePath,
                    CreatedDate = f.CreatedDate
                }).ToList()
                : new List<FeedbackModel>()
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
                isLocationInPlan = await _tourPlanService.IsLocationInPlanAsync(userId.Value, id);
            }

            ViewBag.IsLocationInPlan = isLocationInPlan;

            return View(location);
        }
        #endregion
    }
}