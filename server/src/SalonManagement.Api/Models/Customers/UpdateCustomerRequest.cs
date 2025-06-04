using FluentValidation;
using SalonManagement.Api.Validation;
using SalonManagement.Api.Validation.Validators;

namespace SalonManagement.Api.Models.Customers;

public class UpdateCustomerRequest : IValidatable<UpdateCustomerRequest>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public IValidator<UpdateCustomerRequest> GetValidator() 
        => new UpdateCustomerRequestValidator();
} 