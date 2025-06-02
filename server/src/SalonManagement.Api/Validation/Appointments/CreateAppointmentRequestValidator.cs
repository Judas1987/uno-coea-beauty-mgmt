using FluentValidation;
using SalonManagement.Api.Models.Appointments;

namespace SalonManagement.Api.Validation.Appointments;

public class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequest>
{
    public CreateAppointmentRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("Customer ID must be provided");

        RuleFor(x => x.ServiceId)
            .GreaterThan(0)
            .WithMessage("Service ID must be provided");

        RuleFor(x => x.StartTime)
            .GreaterThan(DateTime.Now)
            .WithMessage("Appointment time must be in the future");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => x.Notes != null)
            .WithMessage("Notes cannot exceed 500 characters");
    }
} 