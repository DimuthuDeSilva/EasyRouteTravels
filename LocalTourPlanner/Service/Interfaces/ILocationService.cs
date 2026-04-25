using LocalTourPlanner.Domain;

namespace LocalTourPlanner.Service.Interfaces
{
    public interface ILocationService
    {
        Task<List<Location>> GetAllLocationAsync();
    }
}
