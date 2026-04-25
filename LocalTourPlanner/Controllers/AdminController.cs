using LocalTourPlanner.Data;
using LocalTourPlanner.Domain;
using LocalTourPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocalTourPlanner.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // --- AUTH CHECK HELPER ---
        private bool IsAdmin() => HttpContext.Session.GetString("UserRole") == "Admin";

        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            return View();
        }

        // ==========================================
        // --- LOCATION MANAGEMENT ---
        // ==========================================

        public IActionResult ManageLocations()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            var locations = _context.Locations.ToList();
            return View(locations);
        }

        public IActionResult CreateLocation()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLocation(Location location, IFormFile? imageFile)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    location.ImagePath = await SaveImage(imageFile);
                }

                _context.Locations.Add(location);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Location added successfully!";
                return RedirectToAction(nameof(ManageLocations));
            }
            return View(location);
        }

        public async Task<IActionResult> EditLocation(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            var location = await _context.Locations.FindAsync(id);
            if (location == null) return NotFound();
            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLocation(Location location, IFormFile? imageFile)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    location.ImagePath = await SaveImage(imageFile);
                }
                // Note: If imageFile is null, the hidden input in the View 
                // preserves the existing ImagePath currently attached to the model.

                _context.Update(location);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Location updated successfully!";
                return RedirectToAction(nameof(ManageLocations));
            }
            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteLocation(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            var loc = _context.Locations.Find(id);
            if (loc != null)
            {
                _context.Locations.Remove(loc);
                _context.SaveChanges();
                TempData["Success"] = "Location deleted successfully!";
            }
            return RedirectToAction(nameof(ManageLocations));
        }

        // ==========================================
        // --- VEHICLE MANAGEMENT ---
        // ==========================================

        public IActionResult ManageVehicles()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            var vehicles = _context.Vehicles.ToList();
            return View(vehicles);
        }

        public IActionResult CreateVehicle()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVehicle(Vehicle vehicle)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Vehicle added successfully!";
                return RedirectToAction(nameof(ManageVehicles));
            }
            return View(vehicle);
        }

        public async Task<IActionResult> EditVehicle(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();
            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVehicle(Vehicle vehicle)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                _context.Update(vehicle);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Vehicle updated successfully!";
                return RedirectToAction(nameof(ManageVehicles));
            }
            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteVehicle(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            var vehicle = _context.Vehicles.Find(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                _context.SaveChanges();
                TempData["Success"] = "Vehicle deleted successfully!";
            }
            return RedirectToAction(nameof(ManageVehicles));
        }

        // --- IMAGE UPLOAD HELPER ---
        private async Task<string> SaveImage(IFormFile file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string path = Path.Combine(wwwRootPath, @"images/locations");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return @"/images/locations/" + fileName;
        }
    }
}