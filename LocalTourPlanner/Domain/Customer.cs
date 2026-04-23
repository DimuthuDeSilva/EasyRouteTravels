using System.ComponentModel.DataAnnotations;

namespace LocalTourPlanner.Domain
{
    public class Customer
    {
        [Key]
        public int? CID { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? UserPassword { get; set; }
        public string? LastLogin { get; set; }
        public string? Review { get; set; }
        public string? UserRole { get; set; }
    }
}
