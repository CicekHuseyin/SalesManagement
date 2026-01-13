using SalesManagement.Models;

namespace SalesManagement.Services.Interfaces;

public interface ICustomerService
{
    IEnumerable<Customer> GetCustomers();
    void AddCustomer(Customer customer);
    void UpdateCustomer(Customer customer);
}
