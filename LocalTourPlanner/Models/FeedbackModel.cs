
namespace LocalTourPlanner.Models
{
    public class FeedbackModel
    {
        public int FeedbackID { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}