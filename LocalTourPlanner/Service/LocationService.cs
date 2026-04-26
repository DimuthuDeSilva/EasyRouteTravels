using LocalTourPlanner.Data;
using LocalTourPlanner.Domain;
using LocalTourPlanner.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocalTourPlanner.Service
{
    public class LocationService : ILocationService
    {
        #region Fields
        private readonly ApplicationDbContext _context;
        #endregion

        #region Ctor
        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Location Methods
        public async Task<List<Location>> GetAllLocationAsync() 
        { 
            return await _context.Locations.Include(l => l.Feedbacks)
                .ToListAsync();
        }
        public async Task<List<Location>> GetAllAsync()
        {
            return await _context.Locations
                .AsNoTracking()
                .ToListAsync();
        }

        // ================= GET BY ID =================
        public async Task<Location?> GetByIdAsync(int id)
        {
            return await _context.Locations
                .FirstOrDefaultAsync(l => l.LID == id);
        }

        // ================= CREATE =================
        public async Task CreateAsync(Location location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();
        }

        // ================= UPDATE =================
        public async Task UpdateAsync(Location location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            var existing = await _context.Locations.FindAsync(location.LID);
            if (existing == null)
                throw new KeyNotFoundException("Location not found");

            // Update fields (manual mapping is safer)
            existing.LocationName = location.LocationName;
            existing.LocationDescription = location.LocationDescription;
            existing.Category = location.Category;
            existing.Latitude = location.Latitude;
            existing.Longitude = location.Longitude;

            // Only update image if provided
            if (!string.IsNullOrEmpty(location.ImagePath))
            {
                existing.ImagePath = location.ImagePath;
            }

            _context.Locations.Update(existing);
            await _context.SaveChangesAsync();
        }

        // ================= DELETE =================
        public async Task DeleteAsync(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
                throw new KeyNotFoundException("Location not found");

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region ImagePath CRUD Methods

        // ✅ GET - Get all images for a specific location
        public async Task<List<ImagePath>> GetImagePathsByLocationIdAsync(int locationId)
        {
            return await _context.ImagePath
                .Where(i => i.LocationID == locationId)
                .ToListAsync();
        }

        // ✅ INSERT - Add new image
        public async Task<bool> InsertImagePathAsync(ImagePath image)
        {
            await _context.ImagePath.AddAsync(image);
            return await _context.SaveChangesAsync() > 0;
        }

        // ✅ UPDATE - Update existing image path
        public async Task<bool> UpdateImagePathAsync(ImagePath image)
        {
            var existing = await _context.ImagePath
                .FirstOrDefaultAsync(i => i.ImageID == image.ImageID);

            if (existing == null)
                return false;

            existing.ImagePathValue = image.ImagePathValue;
            existing.LocationID = image.LocationID;

            _context.ImagePath.Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }

        // ✅ DELETE - Delete image by ID
        public async Task<bool> DeleteImagePathAsync(int imageId)
        {
            var image = await _context.ImagePath
                .FirstOrDefaultAsync(i => i.ImageID == imageId);

            if (image == null)
                return false;

            _context.ImagePath.Remove(image);
            return await _context.SaveChangesAsync() > 0;
        }

        #endregion
    }
}