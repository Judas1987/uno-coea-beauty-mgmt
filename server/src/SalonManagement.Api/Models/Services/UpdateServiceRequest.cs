using System.ComponentModel.DataAnnotations;

namespace SalonManagement.Api.Models.Services;

public class UpdateServiceRequest
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Range(0, int.MaxValue)]
    public int? DurationMinutes { get; set; }

    public decimal? PromotionalPrice { get; set; }
} 