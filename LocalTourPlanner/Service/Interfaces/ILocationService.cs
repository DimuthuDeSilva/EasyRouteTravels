using LocalTourPlanner.Domain;

namespace LocalTourPlanner.Service.Interfaces
{
    public interface ILocationService
    {
        Task<List<Location>> GetAllLocationAsync();
        Task<List<ImagePath>> GetImagePathsByLocationIdAsync(int locationId);
        Task<bool> InsertImagePathAsync(ImagePath image);
        Task<bool> UpdateImagePathAsync(ImagePath image);
        Task<bool> DeleteImagePathAsync(int imageId);
        Task<List<Location>> GetAllAsync();
        Task<Location?> GetByIdAsync(int id);
        Task CreateAsync(Location location);
        Task UpdateAsync(Location location);
        Task DeleteAsync(int id);
    }
}
