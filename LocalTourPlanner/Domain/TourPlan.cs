using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocalTourPlanner.Domain
{
    public class TourPlan
    {
        [Key]
        public int PlanID { get; set; }

        public int CustomerID { get; set; } // Who is planning?

        public int LocationID { get; set; } // Where are they going?

        public DateTime DateAdded { get; set; } = DateTime.Now;

        // Navigation properties (optional but helpful for BIT projects)
        [ForeignKey("LocationID")]
        public virtual Location? Location { get; set; }
    }
}