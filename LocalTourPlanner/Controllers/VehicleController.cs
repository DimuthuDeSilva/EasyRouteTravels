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

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleModel model)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Index", "Home");

            var existingVehicles = await _vehicleService.GetAllVehicleAsync();

            if (existingVehicles.Any(v =>
                v.VehicleName.Equals(model.VehicleName, StringComparison.OrdinalIgnoreCase)))
            {
                TempData["Error"] = "A vehicle with this name already exists!";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var vehicle = new Vehicle
                {
                    VehicleName = model.VehicleName,
                    VehicleType = model.VehicleType,
                    Rate = model.Rate,
                    SeatingCapacity = model.SeatingCapacity
                };

                await _vehicleService.InsertVehicleAsync(vehicle);

                TempData["Success"] = "Vehicle added successfully!";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Index", "Home");

            var data = await _vehicleService.GetByIdAsync(id);
            if (data == null) return NotFound();

            var model = new VehicleModel
            {
                VID = data.VID,
                VehicleName = data.VehicleName,
                VehicleType = data.VehicleType,
                Rate = data.Rate,
                SeatingCapacity = data.SeatingCapacity
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehicleModel model)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Index", "Home");

            // Check duplicate name (excluding itself)
            var allVehicles = await _vehicleService.GetAllVehicleAsync();

            if (allVehicles.Any(v =>
                v.VehicleName.Equals(model.VehicleName, StringComparison.OrdinalIgnoreCase)
                && v.VID != model.VID))
            {
                TempData["Error"] = "Another vehicle is already using this name!";
                return View(model);
            }

            var vehicle = new Vehicle
            {
                VID = model.VID ?? 0,
                VehicleName = model.VehicleName,
                VehicleType = model.VehicleType,
                Rate = model.Rate,
                SeatingCapacity = model.SeatingCapacity
            };

            await _vehicleService.UpdateVehicleAsync(vehicle);

            TempData["Success"] = "Vehicle updated successfully!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Index", "Home");

            await _vehicleService.DeleteVehicleAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SelectVehicle(int vehicleId, string vehicleName, decimal rate)
        {
            HttpContext.Session.SetInt32("SelectedVehicleID", vehicleId);
            HttpContext.Session.SetString("SelectedVehicleName", vehicleName);
            HttpContext.Session.SetString("VehicleRate", rate.ToString());

            TempData["Message"] = $"You have selected the {vehicleName}!";

            return RedirectToAction("MyPlan", "Tour");
        }
        #endregion
    }
}