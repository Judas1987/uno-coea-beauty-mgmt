using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalonManagement.Api.Models.Appointments;
using SalonManagement.Dal.Dtos;
using SalonManagement.Services.Interfaces;

namespace SalonManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentsService _appointmentsService;
    private readonly IMapper _mapper;

    public AppointmentsController(IAppointmentsService appointmentsService, IMapper mapper)
    {
        _appointmentsService = appointmentsService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AppointmentViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentViewModel>>> GetAppointments()
    {
        var appointments = await _appointmentsService.GetAllAppointmentsAsync();
        return Ok(_mapper.Map<IEnumerable<AppointmentViewModel>>(appointments));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AppointmentViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentViewModel>> GetAppointment(int id)
    {
        var appointment = await _appointmentsService.GetAppointmentByIdAsync(id);
        if (appointment == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<AppointmentViewModel>(appointment));
    }

    [HttpPost]
    [ProducesResponseType(typeof(AppointmentViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AppointmentViewModel>> CreateAppointment(CreateAppointmentRequest request)
    {
        try
        {
            var appointmentDto = _mapper.Map<AppointmentDto>(request);
            await _appointmentsService.CreateAppointmentAsync(appointmentDto);
            
            var createdAppointment = await _appointmentsService.GetAppointmentByIdAsync(appointmentDto.Id);
            return CreatedAtAction(nameof(GetAppointment), new { id = createdAppointment.Id }, 
                _mapper.Map<AppointmentViewModel>(createdAppointment));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAppointment(int id, CreateAppointmentRequest request)
    {
        try
        {
            var appointmentDto = _mapper.Map<AppointmentDto>(request);
            appointmentDto.Id = id;
            
            await _appointmentsService.UpdateAppointmentAsync(id, appointmentDto);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return ex.Message.Contains("not found") ? NotFound(ex.Message) : BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        try
        {
            await _appointmentsService.DeleteAppointmentAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
} 