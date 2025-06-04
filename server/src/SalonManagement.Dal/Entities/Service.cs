using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonManagement.Dal.Entities;

[Table("services")]
public class Service
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("duration_minutes")]
    public int DurationMinutes { get; set; }

    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("is_promotional")]
    public bool? IsPromotional { get; set; }

    [Column("promotional_price")]
    public decimal? PromotionalPrice { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public ServiceCategory Category { get; set; }

    public ICollection<Appointment> Appointments { get; set; }

    public ICollection<ServicePackage> ServicePackages { get; set; }
}
