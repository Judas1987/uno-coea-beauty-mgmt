using FluentValidation;
using SalonManagement.Api.Models.Services;

namespace SalonManagement.Api.Validation.Validators;

public class CreateServiceRequestValidator : AbstractValidator<CreateServiceRequest>
{
    public CreateServiceRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Category ID must be provided");

        RuleFor(x => x.DurationMinutes)
            .GreaterThanOrEqualTo(0).When(x => x.DurationMinutes.HasValue)
            .WithMessage("Duration minutes must be greater than or equal to 0");
    }
} 