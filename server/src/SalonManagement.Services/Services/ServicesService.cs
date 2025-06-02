using SalonManagement.Dal.Dtos;
using SalonManagement.Services.Interfaces;

namespace SalonManagement.Services.Services;

public class ServicesService : IServicesService
{
    public Task<IEnumerable<ServiceDto>> GetAllServicesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ServiceDto> GetServiceByIdAsync(int serviceId)
    {
        throw new NotImplementedException();
    }

    public Task CreateServiceAsync(ServiceDto service)
    {
        throw new NotImplementedException();
    }

    public Task UpdateServiceAsync(int serviceId, ServiceDto service)
    {
        throw new NotImplementedException();
    }

    public Task DeleteServiceAsync(int serviceId)
    {
        throw new NotImplementedException();
    }
}