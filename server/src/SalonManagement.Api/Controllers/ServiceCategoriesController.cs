using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalonManagement.Api.Models.ServiceCategories;
using SalonManagement.Dal.Dtos;
using SalonManagement.Services.Interfaces;
using SalonManagement.Api.Validation;

namespace SalonManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceCategoriesController : ControllerBase
{
    private readonly IServiceCategoriesService _serviceCategoriesService;
    private readonly IMapper _mapper;

    public ServiceCategoriesController(IServiceCategoriesService serviceCategoriesService, IMapper mapper)
    {
        _serviceCategoriesService = serviceCategoriesService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ServiceCategoryViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ServiceCategoryViewModel>>> GetAllCategories()
    {
        var categories = await _serviceCategoriesService.GetAllCategoriesAsync();
        return Ok(_mapper.Map<IEnumerable<ServiceCategoryViewModel>>(categories));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ServiceCategoryViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceCategoryViewModel>> GetCategoryById(int id)
    {
        var category = await _serviceCategoriesService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<ServiceCategoryViewModel>(category));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ServiceCategoryViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ServiceCategoryViewModel>> CreateCategory([FromBody] CreateServiceCategoryRequest request)
    {
        var errorResult = request.Validate();
        if (errorResult is not null) return BadRequest(errorResult);

        var categoryDto = _mapper.Map<ServiceCategoryDto>(request);
        var createdCategory = await _serviceCategoriesService.CreateCategoryAsync(categoryDto);
        var viewModel = _mapper.Map<ServiceCategoryViewModel>(createdCategory);
        return CreatedAtAction(nameof(GetCategoryById), new { id = viewModel.Id }, viewModel);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateServiceCategoryRequest request)
    {
        var errorResult = request.Validate();
        if (errorResult is not null) return BadRequest(errorResult);

        try
        {
            var categoryDto = _mapper.Map<ServiceCategoryDto>(request);
            await _serviceCategoriesService.UpdateCategoryAsync(id, categoryDto);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            await _serviceCategoriesService.DeleteCategoryAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
