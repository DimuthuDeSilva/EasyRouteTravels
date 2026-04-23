using LocalTourPlanner.Data;
using LocalTourPlanner.Interfaces;
using Microsoft.EntityFrameworkCore;
// Note: We are being very specific about which Customer to use below

namespace LocalTourPlanner.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        // We use LocalTourPlanner.Domain.Customer to match your ApplicationDbContext
        public async Task<bool> RegisterCustomer(LocalTourPlanner.Domain.Customer customer)
        {
            try
            {
                customer.LastLogin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                _context.Customer.Add(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<LocalTourPlanner.Domain.Customer?> Login(string username, string password)
        {
            return await _context.Customer
                .FirstOrDefaultAsync(u => u.UserName == username && u.UserPassword == password);
        }

        public async Task<bool> IsUsernameExists(string username)
        {
            return await _context.Customer.AnyAsync(u => u.UserName == username);
        }
    }
}