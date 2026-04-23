using LocalTourPlanner.Data;
using LocalTourPlanner.Domain;
using Microsoft.EntityFrameworkCore;

namespace LocalTourPlanner.Service
{
    public class VehicleService : IVehicleService
    {
        #region Fields
        private readonly ApplicationDbContext _context;
        #endregion

        #region Ctro
        public VehicleService(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<List<Vehicle>> GetAllVehicleAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }
        public async Task<Vehicle> InsertVehicleAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }
        public async Task<bool> DeleteVehicleAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
                return false;

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateVehicleAsync(Vehicle vehicle)
        {
            var existing = await _context.Vehicles.FindAsync(vehicle.VID);

            if (existing == null)
                return false;

            existing.VehicleName = vehicle.VehicleName;
            existing.VehicleType = vehicle.VehicleType;
            existing.Rate = vehicle.Rate;
            existing.SeatingCapacity = vehicle.SeatingCapacity;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Vehicle?> GetByIdAsync(int id)
        {
            // This finds a specific vehicle by its primary key (VID)
            return await _context.Vehicles.FirstOrDefaultAsync(v => v.VID == id);
        }
        #endregion
    }
}
