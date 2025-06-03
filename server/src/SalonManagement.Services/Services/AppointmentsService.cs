using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalonManagement.Dal;
using SalonManagement.Dal.Dtos;
using SalonManagement.Dal.Entities;
using SalonManagement.Services.Interfaces;

namespace SalonManagement.Services.Services;

public class AppointmentsService : IAppointmentsService
{
    private readonly ISalonDbContext _dbContext;
    private readonly IMapper _mapper;

    public AppointmentsService(ISalonDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
    {
        var appointments = await _dbContext.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Service)
            .ToListAsync();

        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<AppointmentDto?> GetAppointmentByIdAsync(int appointmentId)
    {
        var appointment = await _dbContext.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Service)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);

        return appointment != null ? _mapper.Map<AppointmentDto>(appointment) : null;
    }

    public async Task<AppointmentDto> CreateAppointmentAsync(AppointmentDto appointmentDto)
    {
        var service = await _dbContext.Services.FindAsync(appointmentDto.ServiceId);
        if (service == null)
        {
            throw new ArgumentException($"Service with ID {appointmentDto.ServiceId} not found.");
        }

        var customer = await _dbContext.Customers.FindAsync(appointmentDto.CustomerId);
        if (customer == null)
        {
            throw new ArgumentException($"Customer with ID {appointmentDto.CustomerId} not found.");
        }

        appointmentDto.EndTime = appointmentDto.StartTime.AddMinutes(service.DurationMinutes);

        var existingAppointments = await _dbContext.Appointments.ToListAsync();
        if (existingAppointments.Count == 0)
        {
            var appointment = _mapper.Map<Appointment>(appointmentDto);
            _dbContext.Appointments.Add(appointment);
            await _dbContext.SaveChangesAsync();
            return await GetAppointmentByIdAsync(appointment.Id);
        }

        if (await HasSchedulingConflict(appointmentDto))
        {
            throw new InvalidOperationException("The requested time slot conflicts with an existing appointment.");
        }

        var newAppointment = _mapper.Map<Appointment>(appointmentDto);
        _dbContext.Appointments.Add(newAppointment);
        await _dbContext.SaveChangesAsync();

        return await GetAppointmentByIdAsync(newAppointment.Id);
    }

    public async Task UpdateAppointmentAsync(int appointmentId, AppointmentDto appointmentDto)
    {
        var existingAppointment = await _dbContext.Appointments.FindAsync(appointmentId);
        if (existingAppointment == null)
        {
            throw new ArgumentException($"Appointment with ID {appointmentId} not found.");
        }

        var service = await _dbContext.Services.FindAsync(appointmentDto.ServiceId);
        if (service == null)
        {
            throw new ArgumentException($"Service with ID {appointmentDto.ServiceId} not found.");
        }

        appointmentDto.EndTime = appointmentDto.StartTime.AddMinutes(service.DurationMinutes);

        if (await HasSchedulingConflict(appointmentDto, appointmentId))
        {
            throw new InvalidOperationException("The requested time slot conflicts with an existing appointment.");
        }

        _mapper.Map(appointmentDto, existingAppointment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAppointmentAsync(int appointmentId)
    {
        var appointment = await _dbContext.Appointments.FindAsync(appointmentId);
        if (appointment == null)
        {
            throw new ArgumentException($"Appointment with ID {appointmentId} not found.");
        }

        _dbContext.Appointments.Remove(appointment);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<bool> HasSchedulingConflict(AppointmentDto appointment, int? excludeAppointmentId = null)
    {
        var query = _dbContext.Appointments.Where(a =>
            (a.StartTime < appointment.EndTime && a.EndTime > appointment.StartTime)).AsQueryable();

        if (excludeAppointmentId.HasValue)
        {
            query = query.Where(a => a.Id != excludeAppointmentId.Value);
        }

        return await query.AnyAsync();
    }
}
