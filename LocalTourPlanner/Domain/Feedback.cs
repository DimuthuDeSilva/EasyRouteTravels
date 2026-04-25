using System.ComponentModel.DataAnnotations;

namespace LocalTourPlanner.Domain
{
    public class Feedback
    {
        [Key]
        public int FeedbackID { get; set; }
        public int LocationID { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; } // 1-5
        public string ImagePath { get; set; } // Stores the file path, not the actual image
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}