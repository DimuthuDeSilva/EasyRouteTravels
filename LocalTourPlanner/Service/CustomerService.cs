using LocalTourPlanner.Data;
using LocalTourPlanner.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using LocalTourPlanner.Domain;


namespace LocalTourPlanner.Services
{
    public class CustomerService : ICustomerService
    {
        #region Fields
        private readonly ApplicationDbContext _context;
        #endregion

        #region Ctro
        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<bool> RegisterCustomer(Customer customer)
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

        public async Task<Customer?> Login(string username, string password)
        {
            return await _context.Customer
                .FirstOrDefaultAsync(u => u.UserName == username && u.UserPassword == password);
        }

        public async Task<bool> IsUsernameExists(string username)
        {
            return await _context.Customer.AnyAsync(u => u.UserName == username);
        }
        #endregion
    }
}