using AutoMapper;
using SalonManagement.Api.Models.Appointments;
using SalonManagement.Dal.Dtos;
using SalonManagement.Dal.Entities;

namespace SalonManagement.Api.Mappings;

public class AppointmentMappingProfile : Profile
{
    public AppointmentMappingProfile()
    {
        // Request > DTO
        CreateMap<CreateAppointmentRequest, AppointmentDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "Scheduled"))
            .ForMember(dest => dest.EndTime, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Service, opt => opt.Ignore())
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes ?? string.Empty));
        
        // Entity <-> DTO
        CreateMap<Appointment, AppointmentDto>();
        CreateMap<AppointmentDto, Appointment>();

        // Related entity mappings
        CreateMap<Customer, CustomerDto>();
        CreateMap<Service, ServiceDto>();
        
        // DTO > ViewModel
        CreateMap<AppointmentDto, AppointmentViewModel>()
            .ForMember(dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.Customer != null 
                    ? $"{src.Customer.FirstName} {src.Customer.LastName}"
                    : string.Empty))
            .ForMember(dest => dest.ServiceName,
                opt => opt.MapFrom(src => src.Service != null 
                    ? src.Service.Title 
                    : string.Empty))
            .ForMember(dest => dest.ServicePrice,
                opt => opt.MapFrom(src => src.Service != null
                    ? (src.Service.IsPromotional.GetValueOrDefault(false)
                        ? src.Service.PromotionalPrice ?? src.Service.Price 
                        : src.Service.Price)
                    : 0m));
    }
} 