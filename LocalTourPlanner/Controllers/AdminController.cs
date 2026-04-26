using LocalTourPlanner.Models;
using LocalTourPlanner.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocalTourPlanner.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILocationService _locationService;
        private readonly IVehicleService _vehicleService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(
            ILocationService locationService,
            IVehicleService vehicleService,
            IWebHostEnvironment webHostEnvironment)

        {
            _locationService = locationService;
            _vehicleService = vehicleService;
            _webHostEnvironment = webHostEnvironment;
        }

        // ================= AUTH =================
        private bool IsAdmin() =>
            HttpContext.Session.GetString("UserRole") == "Admin";

        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            return View();
        }

        // ================= LOCATION =================

        public async Task<IActionResult> ManageLocations()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            var locations = await _locationService.GetAllAsync();

            var model = locations.Select(x => new LocationModel
            {
                LID = x.LID,
                LocationName = x.LocationName,
                LocationDescription = x.LocationDescription,
                Category = x.Category,
                Distance = x.Distance,
                ShortDescription = x.ShortDescription,
                ImagePath = x.ImagePath,
                OpeningHours = x.OpeningHours,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            }).ToList();

            return View(model);
        }

        public IActionResult CreateLocation()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateLocation(
    LocationModel model,
    IFormFile? imageFile,
    List<IFormFile>? imageFiles)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                // ================= CREATE LOCATION =================
                var location = new Domain.Location
                {
                    LocationName = model.LocationName,
                    LocationDescription = model.LocationDescription,
                    Category = model.Category,
                    Distance = model.Distance,
                    ShortDescription = model.ShortDescription,
                    OpeningHours = model.OpeningHours,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude
                };

                // Save main image
                if (imageFile != null)
                {
                    location.ImagePath = await SaveImage(imageFile);
                }

                // Save location first to get LID
                await _locationService.CreateAsync(location);

                // ================= GALLERY IMAGES =================
                if (imageFiles != null && imageFiles.Count > 0)
                {
                    foreach (var file in imageFiles)
                    {
                        var path = await SaveGalleryImages(file);

                        var imageEntity = new Domain.ImagePath
                        {
                            LocationID = location.LID,   // IMPORTANT: FK
                            ImagePathValue = path
                        };

                        await _locationService.InsertImagePathAsync(imageEntity);
                    }
                }

                TempData["Success"] = "Location added successfully!";
                return RedirectToAction(nameof(ManageLocations));
            }

            return View(model);
        }
        public async Task<IActionResult> EditLocation(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            var location = await _locationService.GetByIdAsync(id);
            if (location == null) return NotFound();

            var model = new LocationModel
            {
                LID = location.LID,
                LocationName = location.LocationName,
                LocationDescription = location.LocationDescription,
                Category = location.Category,
                Distance = location.Distance,
                ShortDescription = location.ShortDescription,
                ImagePath = location.ImagePath,
                OpeningHours = location.OpeningHours,
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditLocation(LocationModel model, IFormFile? imageFile)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var location = new Domain.Location
                {
                    LID = model.LID ?? 0,
                    LocationName = model.LocationName,
                    LocationDescription = model.LocationDescription,
                    Category = model.Category,
                    Distance = model.Distance,
                    ShortDescription = model.ShortDescription,
                    OpeningHours = model.OpeningHours,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    ImagePath = model.ImagePath
                };

                if (imageFile != null)
                    location.ImagePath = await SaveImage(imageFile);

                await _locationService.UpdateAsync(location);

                TempData["Success"] = "Location updated successfully!";
                return RedirectToAction(nameof(ManageLocations));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            await _locationService.DeleteAsync(id);

            TempData["Success"] = "Location deleted successfully!";
            return RedirectToAction(nameof(ManageLocations));
        }

        // ================= VEHICLES =================

        public async Task<IActionResult> ManageVehicles()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            var vehicles = await _vehicleService.GetAllVehicleAsync();

            var model = vehicles.Select(x => new VehicleModel
            {
                VID = x.VID,
                VehicleName = x.VehicleName,
                VehicleType = x.VehicleType,
                Rate = x.Rate,
                SeatingCapacity = x.SeatingCapacity
            }).ToList();

            return View(model);
        }

        public IActionResult CreateVehicle()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle(VehicleModel model)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var vehicle = new Domain.Vehicle
                {
                    VehicleName = model.VehicleName,
                    VehicleType = model.VehicleType,
                    Rate = model.Rate,
                    SeatingCapacity = model.SeatingCapacity
                };

                await _vehicleService.InsertVehicleAsync(vehicle);

                TempData["Success"] = "Vehicle added successfully!";
                return RedirectToAction(nameof(ManageVehicles));
            }

            return View(model);
        }

        public async Task<IActionResult> EditVehicle(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null) return NotFound();

            var model = new VehicleModel
            {
                VID = vehicle.VID,
                VehicleName = vehicle.VehicleName,
                VehicleType = vehicle.VehicleType,
                Rate = vehicle.Rate,
                SeatingCapacity = vehicle.SeatingCapacity
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditVehicle(VehicleModel model)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var vehicle = new Domain.Vehicle
                {
                    VID = model.VID ?? 0,
                    VehicleName = model.VehicleName,
                    VehicleType = model.VehicleType,
                    Rate = model.Rate,
                    SeatingCapacity = model.SeatingCapacity
                };

                await _vehicleService.UpdateVehicleAsync(vehicle);

                TempData["Success"] = "Vehicle updated successfully!";
                return RedirectToAction(nameof(ManageVehicles));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            await _vehicleService.DeleteVehicleAsync(id);

            TempData["Success"] = "Vehicle deleted successfully!";
            return RedirectToAction(nameof(ManageVehicles));
        }

        // ================= IMAGE HELPER =================

        private async Task<string> SaveImage(IFormFile file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string path = Path.Combine(wwwRootPath, "images/locations");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            await file.CopyToAsync(stream);

            return "/images/locations/" + fileName;
        }
        private async Task<string> SaveGalleryImages(IFormFile file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string path = Path.Combine(wwwRootPath, "images/detailsimages");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            await file.CopyToAsync(stream);

            return "/images/detailsimages/" + fileName;
        }
    }
}