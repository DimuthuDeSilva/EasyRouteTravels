using LocalTourPlanner.Domain;

namespace LocalTourPlanner.Service.Interfaces
{
    public interface IVehicleService
    {
        Task<List<Vehicle>> GetAllVehicleAsync();
        Task<Vehicle?> GetByIdAsync(int id);
        Task<Vehicle> InsertVehicleAsync(Vehicle vehicle);
        Task<bool> UpdateVehicleAsync(Vehicle vehicle);
        Task<bool> DeleteVehicleAsync(int id);
    }
}
