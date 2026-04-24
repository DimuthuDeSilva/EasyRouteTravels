using LocalTourPlanner.Data;
using LocalTourPlanner.Domain;
using LocalTourPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalTourPlanner.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create(int locationId, string locationName)
        {
            // The Security Guard: Block if they haven't finished the plan
            var vehicleRate = HttpContext.Session.GetString("VehicleRate");
            if (string.IsNullOrEmpty(vehicleRate))
            {
                TempData["Error"] = "Please finalize your tour plan and select a vehicle before writing a review.";
                return RedirectToAction("MyPlan", "Tour");
            }

            var model = new FeedbackViewModel
            {
                LocationID = locationId,
                LocationName = locationName
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FeedbackViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string? uniqueFileName = null;

                if (vm.ImageFile != null)
                {
                    // Save Image to wwwroot/uploads
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await vm.ImageFile.CopyToAsync(fileStream);
                    }
                }

                // Map ViewModel to Domain Model
                var feedback = new Feedback
                {
                    LocationID = vm.LocationID,
                    UserName = vm.UserName,
                    Comment = vm.Comment,
                    Rating = vm.Rating,
                    ImagePath = uniqueFileName != null ? "/uploads/" + uniqueFileName : null,
                    CreatedDate = DateTime.Now
                };

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Thank you! Your review has been posted.";
                return RedirectToAction("Details", "Locations", new { id = vm.LocationID });
            }
            return View(vm);
        }
    }
}