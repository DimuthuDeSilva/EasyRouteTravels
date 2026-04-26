using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocalTourPlanner.Domain
{
    public class ImagePath
    {
        [Key]
        public int ImageID { get; set; }

        [Required]
        public int? LocationID { get; set; }

        [Required]
        [StringLength(255)]
        public string ImagePathValue { get; set; }

        // Navigation Property
        [ForeignKey("LocationID")]
        public virtual Location Location { get; set; }
    }
}
