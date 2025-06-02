using SalonManagement.Dal.Dtos;
using SalonManagement.Services.Interfaces;

namespace SalonManagement.Services.Services;

public class AppointmentsService : IAppointmentsService
{
    public Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<AppointmentDto> GetAppointmentByIdAsync(int appointmentId)
    {
        throw new NotImplementedException();
    }

    public Task CreateAppointmentAsync(AppointmentDto appointment)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAppointmentAsync(int appointmentId, AppointmentDto appointment)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAppointmentAsync(int appointmentId)
    {
        throw new NotImplementedException();
    }
}