namespace SalonManagement.Api.Models.Services;

public class ServiceViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? PromotionalPrice { get; set; }
    public bool IsPromotional { get; set; }
    public bool IsActive { get; set; }
    public int CategoryId { get; set; }
    public string CategoryTitle { get; set; } = string.Empty;
    public int? DurationMinutes { get; set; }
    public decimal? CurrentPrice => IsPromotional && PromotionalPrice.HasValue ? PromotionalPrice.Value : Price;
} 