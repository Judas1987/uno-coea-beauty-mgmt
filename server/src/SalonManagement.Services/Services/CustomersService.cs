using SalonManagement.Dal.Dtos;
using SalonManagement.Dal.Entities;
using SalonManagement.Services.Interfaces;

namespace SalonManagement.Services.Services;

public class CustomersService : ICustomersService
{
    public Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<CustomerDto> GetCustomerByIdAsync(int customerId)
    {
        throw new NotImplementedException();
    }

    public Task CreateCustomerAsync(CustomerDto customer)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCustomerAsync(int customerId, CustomerDto customer)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCustomerAsync(int customerId)
    {
        throw new NotImplementedException();
    }
}