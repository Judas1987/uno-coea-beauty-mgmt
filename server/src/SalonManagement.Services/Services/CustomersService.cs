using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalonManagement.Dal;
using SalonManagement.Dal.Dtos;
using SalonManagement.Dal.Entities;
using SalonManagement.Services.Interfaces;

namespace SalonManagement.Services.Services;

public class CustomersService : ICustomersService
{
    private readonly SalonDbContext _dbContext;
    private readonly IMapper _mapper;

    // Loyalty points configuration
    private const int POINTS_PER_VISIT = 10;
    private const int POINTS_PER_REFERRAL = 50;
    private const decimal POINTS_TO_CURRENCY_RATIO = 0.01m; // 100 points = $1

    public CustomersService(SalonDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
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

        // Preserve the existing loyalty points when updating other information
        var existingPoints = customer.LoyaltyPoints;
        _mapper.Map(customerDto, customer);
        customer.LoyaltyPoints = existingPoints;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCustomerAsync(int customerId)
    {
        var customer = await _dbContext.Customers.FindAsync(customerId);
        if (customer == null)
        {
            throw new ArgumentException($"Customer with ID {customerId} not found.");
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

    public async Task<(int remainingPoints, decimal discountAmount)> UsePointsForDiscountAsync(
        int customerId, int pointsToUse)
    {
        var customer = await _dbContext.Customers.FindAsync(customerId);
        if (customer == null)
        {
            throw new ArgumentException($"Customer with ID {customerId} not found.");
        }

        if (pointsToUse > customer.LoyaltyPoints)
        {
            throw new InvalidOperationException("Not enough loyalty points available.");
        }

        if (pointsToUse <= 0)
        {
            throw new ArgumentException("Points to use must be greater than zero.");
        }

        var discountAmount = pointsToUse * POINTS_TO_CURRENCY_RATIO;
        customer.LoyaltyPoints -= pointsToUse;
        await _dbContext.SaveChangesAsync();

        return (customer.LoyaltyPoints, discountAmount);
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