using LocalTourPlanner.Data;
using LocalTourPlanner.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using LocalTourPlanner.Domain;


namespace LocalTourPlanner.Services
{
    public class TourPlanService : ITourPlanService
    {
        #region Fields
        private readonly ApplicationDbContext _context;
        #endregion

        #region Ctro
        public TourPlanService(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task AddToPlanAsync(int customerId, int locationId)
        {
            var exists = await _context.TourPlans
                .AnyAsync(p => p.CustomerID == customerId && p.LocationID == locationId);

            if (!exists)
            {
                var plan = new TourPlan
                {
                    CustomerID = customerId,
                    LocationID = locationId,
                    DateAdded = DateTime.Now
                };

                _context.TourPlans.Add(plan);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsAlreadyInPlanAsync(int customerId, int locationId)
        {
            return await _context.TourPlans
                .AnyAsync(p => p.CustomerID == customerId && p.LocationID == locationId);
        }

        public async Task<List<TourPlan>> GetUserPlansAsync(int customerId)
        {
            return await _context.TourPlans
                .Include(p => p.Location)
                .Where(p => p.CustomerID == customerId)
                .OrderBy(p => p.DateAdded)
                .ToListAsync();
        }
        public async Task RemoveFromPlanAsync(int customerId, int planId)
        {
            var planItem = await _context.TourPlans
                .FirstOrDefaultAsync(p => p.PlanID == planId && p.CustomerID == customerId);

            if (planItem != null)
            {
                _context.TourPlans.Remove(planItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsLocationInPlanAsync(int customerId, int locationId)
        {
            return await _context.TourPlans
                .AnyAsync(tp => tp.CustomerID == customerId && tp.LocationID == locationId);
        }

        public async Task<TourPlan?> GetUserPlanByIdAsync(int customerId, int planId)
        {
            return await _context.TourPlans
                .FirstOrDefaultAsync(p => p.PlanID == planId && p.CustomerID == customerId);
        }
        #endregion
    }
}