using SalonManagement.Dal.Dtos;

namespace SalonManagement.Services.Interfaces;

public interface IAppointmentsService
{
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
    Task<AppointmentDto?> GetAppointmentByIdAsync(int appointmentId);
    Task<AppointmentDto> CreateAppointmentAsync(AppointmentDto appointment);
    Task UpdateAppointmentAsync(int appointmentId, AppointmentDto appointment);
    Task DeleteAppointmentAsync(int appointmentId);
}