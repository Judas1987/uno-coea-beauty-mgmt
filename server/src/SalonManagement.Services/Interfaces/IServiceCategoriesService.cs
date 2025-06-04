using SalonManagement.Dal.Dtos;

namespace SalonManagement.Services.Interfaces;

public interface IServiceCategoriesService
{
    Task<IEnumerable<ServiceCategoryDto>> GetAllCategoriesAsync();
    Task<ServiceCategoryDto?> GetCategoryByIdAsync(int categoryId);
    Task<ServiceCategoryDto> CreateCategoryAsync(ServiceCategoryDto category);
    Task UpdateCategoryAsync(int categoryId, ServiceCategoryDto category);
    Task DeleteCategoryAsync(int categoryId);
}