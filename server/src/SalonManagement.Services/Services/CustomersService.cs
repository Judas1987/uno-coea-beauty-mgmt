using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalonManagement.Dal;
using SalonManagement.Dal.Dtos;
using SalonManagement.Dal.Entities;
using SalonManagement.Services.Interfaces;

namespace SalonManagement.Services.Services;

public class CustomersService : ICustomersService
{
    private readonly ISalonDbContext _dbContext;
    private readonly IMapper _mapper;

    // Loyalty points configuration
    private const int POINTS_PER_VISIT = 10;        // Points earned for each salon visit
    private const int POINTS_PER_REFERRAL = 50;     // Points earned for referring a new customer
    private const decimal POINTS_TO_CURRENCY_RATIO = 0.01m; // 100 points = $1 in discounts

    public CustomersService(ISalonDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        // Return customers sorted by last name, then first name for easy lookup
        var customers = await _dbContext.Customers
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(int customerId)
    {
        var customer = await _dbContext.Customers
            .Include(c => c.Appointments)
            .FirstOrDefaultAsync(c => c.Id == customerId);

        return customer != null ? _mapper.Map<CustomerDto>(customer) : null;
    }

    public async Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto)
    {
        var customer = _mapper.Map<Customer>(customerDto);

        // New customers start with 0 loyalty points
        customer.LoyaltyPoints = 0;

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task UpdateCustomerAsync(int customerId, CustomerDto customerDto)
    {
        var customer = await _dbContext.Customers.FindAsync(customerId);
        if (customer == null)
        {
            throw new ArgumentException($"Customer with ID {customerId} not found.");
        }

        // Preserve the existing loyalty points when updating customer info
        var existingPoints = customer.LoyaltyPoints;
        _mapper.Map(customerDto, customer);
        customer.LoyaltyPoints = existingPoints;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCustomerAsync(int customerId)
    {
        var customer = await _dbContext.Customers
            .Include(c => c.Appointments)
            .FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer == null)
        {
            throw new ArgumentException($"Customer with ID {customerId} not found.");
        }

        if (customer.Appointments.Count != 0)
        {
            throw new InvalidOperationException(
                "Cannot delete customer with existing appointments. Consider marking as inactive instead.");
        }

        _dbContext.Customers.Remove(customer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> AddLoyaltyPointsForVisitAsync(int customerId)
    {
        var customer = await _dbContext.Customers.FindAsync(customerId);
        if (customer == null)
        {
            throw new ArgumentException($"Customer with ID {customerId} not found.");
        }

        customer.LoyaltyPoints += POINTS_PER_VISIT;
        await _dbContext.SaveChangesAsync();

        return customer.LoyaltyPoints;
    }

    public async Task<int> AddLoyaltyPointsForReferralAsync(int customerId)
    {
        var customer = await _dbContext.Customers.FindAsync(customerId);
        if (customer == null)
        {
            throw new ArgumentException($"Customer with ID {customerId} not found.");
        }

        customer.LoyaltyPoints += POINTS_PER_REFERRAL;
        await _dbContext.SaveChangesAsync();

        return customer.LoyaltyPoints;
    }

    public async Task<decimal> GetAvailableDiscountAmountAsync(int customerId)
    {
        var customer = await _dbContext.Customers.FindAsync(customerId);
        if (customer == null)
        {
            throw new ArgumentException($"Customer with ID {customerId} not found.");
        }

        return customer.LoyaltyPoints * POINTS_TO_CURRENCY_RATIO;
    }
}
