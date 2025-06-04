namespace SalonManagement.Api.Models.Customers;

public class CustomerViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int LoyaltyPoints { get; set; }
    public decimal AvailableDiscountAmount { get; set; }
} 