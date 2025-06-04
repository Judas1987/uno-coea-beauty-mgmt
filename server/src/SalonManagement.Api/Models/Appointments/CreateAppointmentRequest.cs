using FluentValidation;
using SalonManagement.Api.Validation;
using SalonManagement.Api.Validation.Appointments;

namespace SalonManagement.Api.Models.Appointments;

public class CreateAppointmentRequest : IValidatable<CreateAppointmentRequest>
{
    public int CustomerId { get; set; }
    public int ServiceId { get; set; }
    public DateTime StartTime { get; set; }
    public string? Notes { get; set; }

    public IValidator<CreateAppointmentRequest> GetValidator() 
        => new CreateAppointmentRequestValidator();
} 