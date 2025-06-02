namespace SalonManagement.Api.Models.Appointments;

public class CreateAppointmentRequest
{
    public int CustomerId { get; set; }
    public int ServiceId { get; set; }
    public DateTime StartTime { get; set; }
    public string? Notes { get; set; }
} 