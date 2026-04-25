using LocalTourPlanner.Domain;

namespace LocalTourPlanner.Service.Interfaces
{
    public interface IFeedbackService
    {
        Task AddFeedbackAsync(Feedback feedback);
        Task UpdateFeedbackAsync(Feedback feedback);
        Task DeleteFeedbackAsync(int feedbackId);
        Task<List<Feedback>> GetAllAsync();
        Task<Feedback?> GetByIdAsync(int id);
    }
}