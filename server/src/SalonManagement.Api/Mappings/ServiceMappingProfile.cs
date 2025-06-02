using AutoMapper;
using SalonManagement.Api.Models.Services;
using SalonManagement.Dal.Dtos;
using SalonManagement.Dal.Entities;

namespace SalonManagement.Api.Mappings;

public class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        // Request > DTO
        CreateMap<CreateServiceRequest, ServiceDto>();
        CreateMap<UpdateServiceRequest, ServiceDto>();

        // DTO > ViewModel
        CreateMap<ServiceDto, ServiceViewModel>()
            .ForMember(dest => dest.CategoryTitle,
                opt => opt.MapFrom(src => src.Category != null ? src.Category.Title : string.Empty))
            .ForMember(dest => dest.IsActive,
                opt => opt.MapFrom(src => src.IsActive ?? true))
            .ForMember(dest => dest.IsPromotional,
                opt => opt.MapFrom(src => src.IsPromotional ?? false));
    }
} 