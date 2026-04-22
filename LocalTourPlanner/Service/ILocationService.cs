using LocalTourPlanner.Domain;

namespace LocalTourPlanner.Service
{
    public interface ILocationService
    {
        Task<List<Location>> GetAllLocationAsync();
    }
}
