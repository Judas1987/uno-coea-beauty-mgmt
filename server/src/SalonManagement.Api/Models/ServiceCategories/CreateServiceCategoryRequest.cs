using System.ComponentModel.DataAnnotations;
using FluentValidation;
using SalonManagement.Api.Validation;
using SalonManagement.Api.Validation.Validators;

namespace SalonManagement.Api.Models.ServiceCategories;

public class CreateServiceCategoryRequest : IValidatable<CreateServiceCategoryRequest>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public IValidator<CreateServiceCategoryRequest> GetValidator()
        => new CreateServiceCategoryRequestValidator();
}
