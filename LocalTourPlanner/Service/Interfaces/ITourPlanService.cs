using LocalTourPlanner.Domain;

namespace LocalTourPlanner.Service.Interfaces
{
    public interface ITourPlanService
    {
        Task AddToPlanAsync(int customerId, int locationId);
        Task<bool> IsAlreadyInPlanAsync(int customerId, int locationId);
        Task<List<TourPlan>> GetUserPlansAsync(int customerId);
        Task RemoveFromPlanAsync(int customerId, int planId);
        Task<TourPlan?> GetUserPlanByIdAsync(int customerId, int planId);
        Task<bool> IsLocationInPlanAsync(int customerId, int locationId);
    }
}