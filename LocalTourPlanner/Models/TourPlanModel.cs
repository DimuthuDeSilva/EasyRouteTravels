
namespace LocalTourPlanner.Models
{
    public class TourPlanModel
    {
        public int? LID { get; set; }
        public string? LocationName { get; set; }
        public string? LocationDescription { get; set; }
        public string? Category { get; set; }
        public int? Distance { get; set; }
        public string? ShortDescription { get; set; }
        public string? ImagePath { get; set; }
        public string? OpeningHours { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int PlanID { get; set; }
        public virtual ICollection<FeedbackModel> Feedbacks { get; set; } = new List<FeedbackModel>();
    }
}