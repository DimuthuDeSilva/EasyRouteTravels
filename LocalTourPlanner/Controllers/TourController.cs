using LocalTourPlanner.Domain;
using LocalTourPlanner.Models;
using LocalTourPlanner.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace LocalTourPlanner.Controllers
{
    public class TourController : Controller
    {
        private readonly ITourPlanService _tourPlanService;

        public TourController(ITourPlanService tourPlanService)
        {
            _tourPlanService = tourPlanService;
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
            var alreadyAdded = await _tourPlanService.IsLocationInPlanAsync(userId.Value, locationId);

            if (!alreadyAdded)
            {
                // 3. If NOT added, create the new record
                var planItem = new TourPlan
                {
                    CustomerID = userId.Value,
                    LocationID = locationId,
                    DateAdded = DateTime.Now
                };

                await _tourPlanService.AddToPlanAsync(userId.Value, locationId);
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

            var userPlan = await _tourPlanService.GetUserPlansAsync(userId.Value);

            var model = userPlan.Select(x => new TourPlanModel
            {
                PlanID = x.PlanID,
                LID = x.LocationID,
                LocationName = x.Location?.LocationName,
                LocationDescription = x.Location?.LocationDescription,
                Category = x.Location?.Category,
                Distance = x.Location?.Distance,
                ShortDescription = x.Location?.ShortDescription,
                ImagePath = x.Location?.ImagePath,
                OpeningHours = x.Location?.OpeningHours,
                Latitude = x.Location?.Latitude,
                Longitude = x.Location?.Longitude,
                Feedbacks = x.Location?.Feedbacks.Select(f => new FeedbackModel
                {
                    FeedbackID = f.FeedbackID,
                    LocationID = f.LocationID,
                    Rating = f.Rating,
                    Comment = f.Comment,
                    ImagePath = f.ImagePath,
                    CreatedDate = f.CreatedDate
                }).ToList() ?? new List<FeedbackModel>()
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromPlan(int planId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            await _tourPlanService.RemoveFromPlanAsync(userId.Value, planId);

            TempData["Message"] = "Location removed from your plan.";

            return RedirectToAction("MyPlan");
        }

        public async Task<IActionResult> DownloadQuotation()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var vehicleId = HttpContext.Session.GetInt32("SelectedVehicleID");

            // Get the rate from session
            var rateStr = HttpContext.Session.GetString("VehicleRate");

            // FIX: Check if the rate or vehicle is null before parsing
            if (userId == null || vehicleId == null || string.IsNullOrEmpty(rateStr))
            {
                TempData["Error"] = "Please select a vehicle first.";
                return RedirectToAction("SelectVehicle", "Vehicle");
            }

            // Safely parse the rate now that we know it's not null
            decimal.TryParse(rateStr, out decimal rate);

            var userPlan = await _tourPlanService.GetUserPlansAsync(userId.Value);

            // Calculations...
            double baseDistance = userPlan.Sum(x => x.Location?.Distance ?? 0);
            double returnDistance = userPlan.Any() ? userPlan.Max(x => x.Location?.Distance ?? 0) : 0;
            double totalDistance = baseDistance + returnDistance;

            var model = new CostBreakdownViewModel
            {
                SelectedLocations = userPlan.Select(x => new TourPlanModel
                {
                    LocationName = x.Location?.LocationName,
                    Category = x.Location?.Category,
                    Distance = x.Location?.Distance
                }).ToList(),
                SelectedVehicleName = HttpContext.Session.GetString("SelectedVehicleName") ?? "Unknown Vehicle",
                RatePerKm = (double)rate,
                TotalDistance = totalDistance,
                GrandTotal = totalDistance * (double)rate,
                GeneratedDate = DateTime.Now.ToString("MMMM dd, yyyy")
            };

            return new ViewAsPdf("QuotationPDF", model)
            {
                FileName = $"Trip_Quotation_{DateTime.Now:yyyyMMdd}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }
    }
}