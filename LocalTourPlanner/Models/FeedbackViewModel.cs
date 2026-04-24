using Microsoft.AspNetCore.Http;

namespace LocalTourPlanner.Models
{
    public class FeedbackViewModel
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; } // To show "Reviewing [Place Name]"
        public string UserName { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public IFormFile? ImageFile { get; set; } // For the actual upload
    }
}