using Microsoft.AspNetCore.Mvc;

namespace SalonManagement.Api.Validation;

public static class ValidationExtensions
{
    public static Dictionary<string, string[]>? Validate<T>(this IValidatable<T> request)
    {
        var result = request.GetValidator().Validate((T)request);

        if (!result.IsValid)
        {
            return result
                .Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        }

        return null;
    }
}
