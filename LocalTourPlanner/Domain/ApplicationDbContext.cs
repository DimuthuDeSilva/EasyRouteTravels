using Microsoft.EntityFrameworkCore;
using LocalTourPlanner.Domain;

namespace LocalTourPlanner.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // These represent your MySQL tables
        public DbSet<Location> Locations { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<TourPlan> TourPlans { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
    }
}