using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalonManagement.Api.Models.Customers;
using SalonManagement.Dal.Dtos;
using SalonManagement.Services.Interfaces;

namespace SalonManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomersService _customersService;
    private readonly IMapper _mapper;

    public CustomersController(ICustomersService customersService, IMapper mapper)
    {
        _customersService = customersService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerViewModel>>> GetAllCustomers()
    {
        var customers = await _customersService.GetAllCustomersAsync();
        var viewModels = _mapper.Map<IEnumerable<CustomerViewModel>>(customers);
        
        foreach (var customer in viewModels)
        {
            customer.AvailableDiscountAmount = await _customersService.GetAvailableDiscountAmountAsync(customer.Id);
        }
        
        return Ok(viewModels);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerViewModel>> GetCustomer(int id)
    {
        var customer = await _customersService.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<CustomerViewModel>(customer);
        viewModel.AvailableDiscountAmount = await _customersService.GetAvailableDiscountAmountAsync(id);
        
        return Ok(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerViewModel>> CreateCustomer(CreateCustomerRequest request)
    {
        var customerDto = _mapper.Map<CustomerDto>(request);
        var createdCustomer = await _customersService.CreateCustomerAsync(customerDto);
        var viewModel = _mapper.Map<CustomerViewModel>(createdCustomer);
        viewModel.AvailableDiscountAmount = await _customersService.GetAvailableDiscountAmountAsync(viewModel.Id);

        return CreatedAtAction(nameof(GetCustomer), new { id = viewModel.Id }, viewModel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, UpdateCustomerRequest request)
    {
        var customerDto = _mapper.Map<CustomerDto>(request);
        customerDto.Id = id;

        try
        {
            await _customersService.UpdateCustomerAsync(id, customerDto);
            return NoContent();
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        try
        {
            await _customersService.DeleteCustomerAsync(id);
            return NoContent();
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }

    [HttpPost("{id}/loyalty/visit")]
    public async Task<ActionResult<int>> AddLoyaltyPointsForVisit(int id)
    {
        try
        {
            var points = await _customersService.AddLoyaltyPointsForVisitAsync(id);
            return Ok(points);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }

    [HttpPost("{id}/loyalty/referral")]
    public async Task<ActionResult<int>> AddLoyaltyPointsForReferral(int id)
    {
        try
        {
            var points = await _customersService.AddLoyaltyPointsForReferralAsync(id);
            return Ok(points);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }

    [HttpPost("{id}/loyalty/use")]
    public async Task<ActionResult<object>> UsePointsForDiscount(int id, [FromBody] int pointsToUse)
    {
        try
        {
            var (remainingPoints, discountAmount) = await _customersService.UsePointsForDiscountAsync(id, pointsToUse);
            return Ok(new { remainingPoints, discountAmount });
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
} 