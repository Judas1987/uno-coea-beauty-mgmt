namespace SalonManagement.Dal.Dtos;

public class ServiceDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }
    int CategoryId { get; set; }
    public bool IsActive { get; set; }
    public bool IsPromotional { get; set; }
    public bool PromotionalPrice { get; set; }
}