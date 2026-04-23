using LocalTourPlanner.Data;
using LocalTourPlanner.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocalTourPlanner.Controllers
{
    public class TourController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TourController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddToPlan(int locationId)
        {
            // 1. Get the current UserID from Session
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. CHECK FOR DUPLICATES: 
            // Check if this specific user has already added this specific location
            var alreadyAdded = await _context.TourPlans
                .AnyAsync(p => p.CustomerID == userId && p.LocationID == locationId);

            if (!alreadyAdded)
            {
                // 3. If NOT added, create the new record
                var planItem = new LocalTourPlanner.Domain.TourPlan
                {
                    CustomerID = userId.Value,
                    LocationID = locationId,
                    DateAdded = DateTime.Now
                };

                _context.TourPlans.Add(planItem);
                await _context.SaveChangesAsync();
            }
            // Note: If it WAS already added, we skip the saving part and just 
            // redirect them to the list so they can see it's already there.

            return RedirectToAction("MyPlan");
        }

        public async Task<IActionResult> MyPlan()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch all plans for this user and include the related Location data
            var userPlan = await _context.TourPlans
                .Include(p => p.Location)
                .Where(p => p.CustomerID == userId)
                .OrderBy(p => p.DateAdded)
                .ToListAsync();

            return View(userPlan);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromPlan(int planId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // 1. Find the specific plan item
            var planItem = await _context.TourPlans
                .FirstOrDefaultAsync(p => p.PlanID == planId && p.CustomerID == userId);

            if (planItem != null)
            {
                // 2. Remove it
                _context.TourPlans.Remove(planItem);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Location removed from your plan.";
            }

            // 3. Go back to the list
            return RedirectToAction("MyPlan");
        }
    }
}