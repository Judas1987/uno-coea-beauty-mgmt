using SalonManagement.Dal.Dtos;

namespace SalonManagement.Services.Interfaces;

public interface IServicesService
{
    // CRUD Operations
    Task<IEnumerable<ServiceDto>> GetAllServicesAsync();
    Task<ServiceDto?> GetServiceByIdAsync(int serviceId);
    Task<ServiceDto> CreateServiceAsync(ServiceDto service);
    Task UpdateServiceAsync(int serviceId, ServiceDto service);
    Task DeleteServiceAsync(int serviceId);

    // Category Operations
    Task<IEnumerable<ServiceDto>> GetServicesByCategoryAsync(int categoryId);

    // Promotional Operations
    Task<IEnumerable<ServiceDto>> GetActivePromotionsAsync();
    Task SetPromotionalPriceAsync(int serviceId, decimal? promotionalPrice);
    Task ActivateServiceAsync(int serviceId);
    Task DeactivateServiceAsync(int serviceId);

    // Search Operations
    Task<IEnumerable<ServiceDto>> SearchServicesAsync(string searchTerm);
    Task<IEnumerable<ServiceDto>> GetServicesByPriceRangeAsync(decimal minPrice, decimal maxPrice);
}