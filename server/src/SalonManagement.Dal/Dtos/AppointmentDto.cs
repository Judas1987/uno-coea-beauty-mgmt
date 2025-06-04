using SalonManagement.Dal.Entities;

namespace SalonManagement.Dal.Dtos;

public class AppointmentDto
{
    public int Id { get; set; }
    
    public int CustomerId { get; set; }
    
    public int ServiceId { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public string Status { get; set; } = string.Empty;
    
    public string Notes { get; set; } = string.Empty;
    
    public CustomerDto? Customer { get; set; }
    
    public ServiceDto? Service { get; set; }
}