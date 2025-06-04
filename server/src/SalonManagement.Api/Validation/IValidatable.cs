using FluentValidation;

namespace SalonManagement.Api.Validation;

public interface IValidatable<T>
{
    IValidator<T> GetValidator();
} 