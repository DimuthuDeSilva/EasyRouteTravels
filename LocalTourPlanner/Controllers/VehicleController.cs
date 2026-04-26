using System.Linq;
using LocalTourPlanner.Data;
using LocalTourPlanner.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LocalTourPlanner.Service.Interfaces;
using LocalTourPlanner.Models;

namespace LocalTourPlanner.Controllers
{
    public class VehicleController : Controller
    {
        #region Fields
        private readonly IVehicleService _vehicleService;

        #endregion

        #region Ctor
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        #endregion

        #region Methods
        public async Task<IActionResult> Index()
        {
            var data = await _vehicleService.GetAllVehicleAsync();

            var model = new VehicleViewModel
            {
                Vehicles = data.Select(x => new VehicleModel
                {
                    VID = x.VID,
                    VehicleName = x.VehicleName ?? "",
                    VehicleType = x.VehicleType ?? "",
                    Rate = x.Rate,
                    SeatingCapacity = x.SeatingCapacity
                }).ToList()
            };

            return View("Vehicle", model);
        }

        [HttpPost]
        public IActionResult SelectVehicle(int vehicleId, string vehicleName, decimal rate)
        {
            HttpContext.Session.SetInt32("SelectedVehicleID", vehicleId);
            HttpContext.Session.SetString("SelectedVehicleName", vehicleName);
            // Store rate as a string to preserve decimal precision in session
            HttpContext.Session.SetString("VehicleRate", rate.ToString());

            TempData["Message"] = $"Vehicle {vehicleName} selected! You can now generate your quotation.";

            return RedirectToAction("MyPlan", "Tour");
        }
        #endregion
    }
}