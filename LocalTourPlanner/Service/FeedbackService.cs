using LocalTourPlanner.Data;
using LocalTourPlanner.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using LocalTourPlanner.Domain;


namespace LocalTourPlanner.Services
{
    public class FeedbackService : IFeedbackService
    {
        #region Fields
        private readonly ApplicationDbContext _context;
        #endregion

        #region Ctro
        public FeedbackService(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task AddFeedbackAsync(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFeedbackAsync(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFeedbackAsync(int feedbackId)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Feedback>> GetAllAsync()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        public async Task<Feedback?> GetByIdAsync(int id)
        {
            return await _context.Feedbacks.FirstOrDefaultAsync(x => x.FeedbackID == id);
        }

        #endregion
    }
}