using System.Linq;
using LocalTourPlanner.Data;
using LocalTourPlanner.Domain;
using LocalTourPlanner.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace LocalTourPlanner.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        // --- CUSTOMER & ADMIN VIEW ---
        public async Task<IActionResult> Index()
        {
            var data = await _vehicleService.GetAllVehicleAsync();

            var model = new LocalTourPlanner.Models.VehicleViewModel
            {
                Vehicles = data.Select(x => new LocalTourPlanner.Models.Vehicle
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

        // --- CREATE (ADMIN ONLY) ---
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Index", "Home");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Index", "Home");

            // Check for duplicate name
            var existingVehicles = await _vehicleService.GetAllVehicleAsync();
            if (existingVehicles.Any(v => v.VehicleName.Equals(vehicle.VehicleName, StringComparison.OrdinalIgnoreCase)))
            {
                TempData["Error"] = "A vehicle with this name already exists!";
                return View(vehicle);
            }

            if (ModelState.IsValid)
            {
                await _vehicleService.InsertVehicleAsync(vehicle);
                TempData["Success"] = "Vehicle added successfully!";
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        // --- EDIT (ADMIN ONLY) ---
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Index", "Home");

            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null) return NotFound();

            return View(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Vehicle vehicle)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Index", "Home");

            // Check for duplicate name (excluding itself)
            var allVehicles = await _vehicleService.GetAllVehicleAsync();
            if (allVehicles.Any(v => v.VehicleName.Equals(vehicle.VehicleName, StringComparison.OrdinalIgnoreCase) && v.VID != vehicle.VID))
            {
                TempData["Error"] = "Another vehicle is already using this name!";
                return View(vehicle);
            }

            await _vehicleService.UpdateVehicleAsync(vehicle);
            TempData["Success"] = "Vehicle updated successfully!";
            return RedirectToAction("Index");
        }

        // --- DELETE (ADMIN ONLY) ---
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Index", "Home");

            await _vehicleService.DeleteVehicleAsync(id);
            return RedirectToAction("Index");
        }

        // --- SELECTION (CUSTOMER ONLY) ---
        [HttpPost]
        public IActionResult SelectVehicle(int vehicleId, string vehicleName, decimal rate)
        {
            HttpContext.Session.SetInt32("SelectedVehicleID", vehicleId);
            HttpContext.Session.SetString("SelectedVehicleName", vehicleName);
            HttpContext.Session.SetString("VehicleRate", rate.ToString());

            TempData["Message"] = $"You have selected the {vehicleName}!";

            return RedirectToAction("MyPlan", "Tour");
        }
    }
}