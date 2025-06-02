using SalonManagement.Dal.Dtos;

namespace SalonManagement.Services.Interfaces;

public interface ICustomersService
{
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerDto?> GetCustomerByIdAsync(int customerId);
    Task<CustomerDto> CreateCustomerAsync(CustomerDto customer);
    Task UpdateCustomerAsync(int customerId, CustomerDto customer);
    Task DeleteCustomerAsync(int customerId);

    Task<int> AddLoyaltyPointsForVisitAsync(int customerId);
    Task<int> AddLoyaltyPointsForReferralAsync(int customerId);
    Task<(int remainingPoints, decimal discountAmount)> UsePointsForDiscountAsync(int customerId, int pointsToUse);
    Task<decimal> GetAvailableDiscountAmountAsync(int customerId);
}