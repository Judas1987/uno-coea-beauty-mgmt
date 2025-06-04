using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalonManagement.Api.Models.Services;
using SalonManagement.Api.Models.ServiceCategories;
using SalonManagement.Dal.Dtos;
using SalonManagement.Services.Interfaces;
using SalonManagement.Api.Validation;

namespace SalonManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IServicesService _servicesService;
    private readonly IServiceCategoriesService _categoriesService;
    private readonly IMapper _mapper;

    public ServicesController(
        IServicesService servicesService,
        IServiceCategoriesService categoriesService,
        IMapper mapper)
    {
        _servicesService = servicesService;
        _categoriesService = categoriesService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ServiceViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ServiceViewModel>>> GetAllServices()
    {
        var services = await _servicesService.GetAllServicesAsync();
        return Ok(_mapper.Map<IEnumerable<ServiceViewModel>>(services));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ServiceViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceViewModel>> GetServiceById(int id)
    {
        var service = await _servicesService.GetServiceByIdAsync(id);
        if (service == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<ServiceViewModel>(service));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ServiceViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ServiceViewModel>> CreateService([FromBody] CreateServiceRequest request)
    {
        var errorResult = request.Validate();
        if (errorResult is not null) return BadRequest(errorResult);

        try
        {
            var serviceDto = _mapper.Map<ServiceDto>(request);
            var createdService = await _servicesService.CreateServiceAsync(serviceDto);
            var viewModel = _mapper.Map<ServiceViewModel>(createdService);
            return CreatedAtAction(nameof(GetServiceById), new { id = viewModel.Id }, viewModel);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateService(int id, UpdateServiceRequest request)
    {
        var errorResult = request.Validate();
        if (errorResult is not null) return BadRequest(errorResult);

        try
        {
            var serviceDto = _mapper.Map<ServiceDto>(request);
            await _servicesService.UpdateServiceAsync(id, serviceDto);
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
    public async Task<IActionResult> DeleteService(int id)
    {
        try
        {
            await _servicesService.DeleteServiceAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("category/{categoryId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ServiceViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ServiceViewModel>>> GetServicesByCategory(int categoryId)
    {
        var services = await _servicesService.GetServicesByCategoryAsync(categoryId);
        return Ok(_mapper.Map<IEnumerable<ServiceViewModel>>(services));
    }

    [HttpGet("promotions")]
    [ProducesResponseType(typeof(IEnumerable<ServiceViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ServiceViewModel>>> GetActivePromotions()
    {
        var services = await _servicesService.GetActivePromotionsAsync();
        return Ok(_mapper.Map<IEnumerable<ServiceViewModel>>(services));
    }

    [HttpPatch("{id:int}/promotional-price")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetPromotionalPrice(int id, [FromBody] decimal? promotionalPrice)
    {
        try
        {
            await _servicesService.SetPromotionalPriceAsync(id, promotionalPrice);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPatch("{id:int}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActivateService(int id)
    {
        try
        {
            await _servicesService.ActivateServiceAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPatch("{id:int}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateService(int id)
    {
        try
        {
            await _servicesService.DeactivateServiceAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
} 