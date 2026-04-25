using LocalTourPlanner.Domain;

namespace LocalTourPlanner.Service.Interfaces
{
    public interface ICustomerService
    {
        Task<bool> RegisterCustomer(Customer customer);
        Task<Customer?> Login(string username, string password);
        Task<bool> IsUsernameExists(string username);
    }
}