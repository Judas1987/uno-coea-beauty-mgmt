using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonManagement.Dal.Entities;

[Table("appointments")]
public class Appointment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("customer_id")]
    public int CustomerId { get; set; }
    
    [Column("service_id")]
    public int ServiceId { get; set; }
    
    [Column("start_time")]
    public DateTime StartTime { get; set; }
    
    [Column("end_time")]
    public DateTime EndTime { get; set; }
    
    [Column("status")]
    public string? Status { get; set; }
    
    [Column("notes")]
    public string? Notes { get; set; }
    
    [ForeignKey(nameof(CustomerId))]
    public Customer Customer { get; set; }
    
    [ForeignKey(nameof(ServiceId))]
    public Service Service { get; set; }
}