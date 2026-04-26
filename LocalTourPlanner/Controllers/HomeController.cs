using LocalTourPlanner.Models;
using LocalTourPlanner.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocalTourPlanner.Controllers
{
    public class HomeController : Controller
    {
        #region Fields
        private readonly ILocationService _locationService;

        #endregion

        #region Ctor
        public HomeController(ILocationService locationService)
        {
            _locationService = locationService;
        }
        #endregion

        #region Methods
        public async Task<IActionResult> Index()
        {
            var data = await _locationService.GetAllLocationAsync();

            var model = new LocationViewModel
            {
                Locations = data.Select(x => new LocationModel
                {
                    LID = x.LID,
                    LocationName = x.LocationName,
                    LocationDescription = x.LocationDescription,
                    ShortDescription = x.ShortDescription,
                    Category = x.Category,
                    Distance = x.Distance,
                    ImagePath =x.ImagePath
                }).ToList()
            };

            return View(model);
        }

        #endregion
    }
}