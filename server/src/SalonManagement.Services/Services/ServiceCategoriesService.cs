using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalonManagement.Dal;
using SalonManagement.Dal.Dtos;
using SalonManagement.Dal.Entities;
using SalonManagement.Services.Interfaces;

namespace SalonManagement.Services.Services;

public class ServiceCategoriesService : IServiceCategoriesService
{
    private readonly ISalonDbContext _dbContext;
    private readonly IMapper _mapper;

    public ServiceCategoriesService(ISalonDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ServiceCategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _dbContext.ServiceCategories
            .ToListAsync();

        return _mapper.Map<IEnumerable<ServiceCategoryDto>>(categories);
    }

    public async Task<ServiceCategoryDto?> GetCategoryByIdAsync(int categoryId)
    {
        var category = await _dbContext.ServiceCategories
            .Include(c => c.Services)
            .FirstOrDefaultAsync(c => c.Id == categoryId);

        return category != null ? _mapper.Map<ServiceCategoryDto>(category) : null;
    }

    public async Task<ServiceCategoryDto> CreateCategoryAsync(ServiceCategoryDto categoryDto)
    {
        var category = _mapper.Map<ServiceCategory>(categoryDto);

        _dbContext.ServiceCategories.Add(category);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<ServiceCategoryDto>(category);
    }

    public async Task UpdateCategoryAsync(int categoryId, ServiceCategoryDto categoryDto)
    {
        var category = await _dbContext.ServiceCategories.FindAsync(categoryId);
        if (category == null)
        {
            throw new ArgumentException($"Service category with ID {categoryId} not found.");
        }

        category.Title = categoryDto.Title;
        category.Description = categoryDto.Description;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int categoryId)
    {
        var category = await _dbContext.ServiceCategories.FindAsync(categoryId);
        if (category == null)
        {
            throw new ArgumentException($"Service category with ID {categoryId} not found.");
        }

        var hasServices = await _dbContext.Services.AnyAsync(s => s.CategoryId == categoryId);

        if (hasServices)
        {
            throw new InvalidOperationException($"Cannot delete category with ID {categoryId} because it has associated services.");
        }

        _dbContext.ServiceCategories.Remove(category);
        await _dbContext.SaveChangesAsync();
    }
}
