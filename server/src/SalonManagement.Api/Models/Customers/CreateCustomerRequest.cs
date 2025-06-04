using FluentValidation;
using SalonManagement.Api.Validation;
using SalonManagement.Api.Validation.Validators;

namespace SalonManagement.Api.Models.Customers;

public class CreateCustomerRequest : IValidatable<CreateCustomerRequest>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public IValidator<CreateCustomerRequest> GetValidator() 
        => new CreateCustomerRequestValidator();
} 