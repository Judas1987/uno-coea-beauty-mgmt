using FluentValidation;
using SalonManagement.Api.Validation;
using SalonManagement.Api.Validation.Validators;

namespace SalonManagement.Api.Models.Services;

public class CreateServiceRequest : IValidatable<CreateServiceRequest>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public int? DurationMinutes { get; set; }

    public IValidator<CreateServiceRequest> GetValidator() 
        => new CreateServiceRequestValidator();
} 