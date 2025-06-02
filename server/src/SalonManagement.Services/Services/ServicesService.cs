using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalonManagement.Dal;
using SalonManagement.Dal.Dtos;
using SalonManagement.Dal.Entities;
using SalonManagement.Services.Interfaces;

namespace SalonManagement.Services.Services;

public class ServicesService : IServicesService
{
    private readonly SalonDbContext _dbContext;
    private readonly IMapper _mapper;

    public ServicesService(SalonDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ServiceDto>> GetAllServicesAsync()
    {
        var services = await _dbContext.Services
            .Include(s => s.Category)
            .OrderBy(s => s.Title)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ServiceDto>>(services);
    }

    public async Task<ServiceDto?> GetServiceByIdAsync(int serviceId)
    {
        var service = await _dbContext.Services
            .Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.Id == serviceId);

        return service != null ? _mapper.Map<ServiceDto>(service) : null;
    }

    public async Task<ServiceDto> CreateServiceAsync(ServiceDto serviceDto)
    {
        await ValidateServiceCategory(serviceDto.CategoryId);

        var service = _mapper.Map<Service>(serviceDto);
        
        service.IsActive = true;
        service.IsPromotional = false;
        service.PromotionalPrice = null;

        _dbContext.Services.Add(service);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<ServiceDto>(service);
    }

    public async Task UpdateServiceAsync(int serviceId, ServiceDto serviceDto)
    {
        var service = await _dbContext.Services
            .Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.Id == serviceId);
            
        if (service == null)
        {
            throw new ArgumentException($"Service with ID {serviceId} not found.");
        }

        await ValidateServiceCategory(serviceDto.CategoryId);
        
        // Manually update the properties
        service.Title = serviceDto.Title;
        service.Description = serviceDto.Description;
        service.Price = serviceDto.Price;
        service.DurationMinutes = serviceDto.DurationMinutes;
        service.CategoryId = serviceDto.CategoryId;
        service.IsActive = serviceDto.IsActive ?? true;
        service.IsPromotional = serviceDto.IsPromotional ?? false;
        service.PromotionalPrice = serviceDto.PromotionalPrice;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteServiceAsync(int serviceId)
    {
        var service = await _dbContext.Services.FindAsync(serviceId);
        if (service == null)
        {
            throw new ArgumentException($"Service with ID {serviceId} not found.");
        }

        // Check if service has any appointments
        var hasAppointments = await _dbContext.Appointments
            .AnyAsync(a => a.ServiceId == serviceId);

        if (hasAppointments)
        {
            // Instead of deleting, deactivate the service
            service.IsActive = false;
        }
        else
        {
            _dbContext.Services.Remove(service);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ServiceDto>> GetServicesByCategoryAsync(int categoryId)
    {
        var services = await _dbContext.Services
            .Include(s => s.Category)
            .Where(s => s.CategoryId == categoryId && (s.IsActive == null || s.IsActive == true))
            .OrderBy(s => s.Title)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ServiceDto>>(services);
    }

    public async Task<IEnumerable<ServiceDto>> GetActivePromotionsAsync()
    {
        var promotions = await _dbContext.Services
            .Include(s => s.Category)
            .Where(s => (s.IsActive == null || s.IsActive == true) && 
                       (s.IsPromotional == true))
            .OrderBy(s => s.Title)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ServiceDto>>(promotions);
    }

    public async Task SetPromotionalPriceAsync(int serviceId, decimal? promotionalPrice)
    {
        var service = await _dbContext.Services.FindAsync(serviceId);
        if (service == null)
        {
            throw new ArgumentException($"Service with ID {serviceId} not found.");
        }

        if (promotionalPrice.HasValue && promotionalPrice.Value >= service.Price)
        {
            throw new ArgumentException("Promotional price must be lower than regular price.");
        }

        service.IsPromotional = promotionalPrice.HasValue;
        service.PromotionalPrice = promotionalPrice;

        await _dbContext.SaveChangesAsync();
    }

    public async Task ActivateServiceAsync(int serviceId)
    {
        var service = await _dbContext.Services.FindAsync(serviceId);
        if (service == null)
        {
            throw new ArgumentException($"Service with ID {serviceId} not found.");
        }

        service.IsActive = true;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeactivateServiceAsync(int serviceId)
    {
        var service = await _dbContext.Services.FindAsync(serviceId);
        if (service == null)
        {
            throw new ArgumentException($"Service with ID {serviceId} not found.");
        }

        service.IsActive = false;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ServiceDto>> SearchServicesAsync(string searchTerm)
    {
        var searchPattern = $"%{searchTerm}%";
        
        var services = await _dbContext.Services
            .Include(s => s.Category)
            .Where(s => (s.IsActive == null || s.IsActive == true) &&
                (EF.Functions.ILike(s.Title, searchPattern) ||
                 EF.Functions.ILike(s.Description, searchPattern) ||
                 EF.Functions.ILike(s.Category.Title, searchPattern)))
            .OrderBy(s => s.Title)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ServiceDto>>(services);
    }

    public async Task<IEnumerable<ServiceDto>> GetServicesByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        var services = await _dbContext.Services
            .Include(s => s.Category)
            .Where(s => (s.IsActive == null || s.IsActive == true) &&
                ((s.IsPromotional == true && s.PromotionalPrice >= minPrice && s.PromotionalPrice <= maxPrice) ||
                 (s.IsPromotional != true && s.Price >= minPrice && s.Price <= maxPrice)))
            .OrderBy(s => s.IsPromotional == true ? s.PromotionalPrice : s.Price)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ServiceDto>>(services);
    }

    private async Task ValidateServiceCategory(int categoryId)
    {
        var categoryExists = await _dbContext.ServiceCategories
            .AnyAsync(c => c.Id == categoryId);

        if (!categoryExists)
        {
            throw new ArgumentException($"Service category with ID {categoryId} not found.");
        }
    }
}