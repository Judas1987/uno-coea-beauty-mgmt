using SalonManagement.Dal.Dtos;
using SalonManagement.Dal.Entities;

namespace SalonManagement.Services.Interfaces;

public interface ICustomersService
{
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerDto> GetCustomerByIdAsync(int customerId);
    Task CreateCustomerAsync(CustomerDto customer);
    Task UpdateCustomerAsync(int customerId, CustomerDto customer);
    Task DeleteCustomerAsync(int customerId);
}