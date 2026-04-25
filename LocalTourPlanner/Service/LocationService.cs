using LocalTourPlanner.Data;
using LocalTourPlanner.Domain;
using LocalTourPlanner.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocalTourPlanner.Service
{
    public class LocationService: ILocationService
    {
        #region Fields
        private readonly ApplicationDbContext _context;
        #endregion

        #region Ctro
        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<List<Location>> GetAllLocationAsync()
        {
            return await _context.Locations
                .Include(l => l.Feedbacks) 
                .ToListAsync();
        }

        #endregion
    }
}
