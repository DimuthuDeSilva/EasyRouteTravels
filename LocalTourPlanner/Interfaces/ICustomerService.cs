using LocalTourPlanner.Models;

namespace LocalTourPlanner.Interfaces
{
    public interface ICustomerService
    {
        Task<bool> RegisterCustomer(LocalTourPlanner.Domain.Customer customer);
        Task<LocalTourPlanner.Domain.Customer?> Login(string username, string password);
        Task<bool> IsUsernameExists(string username);
    }
}