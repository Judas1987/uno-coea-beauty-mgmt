using AutoMapper;
using SalonManagement.Dal.Dtos;
using SalonManagement.Dal.Entities;

namespace SalonManagement.Services.Mappings;

public class ServiceCategoryMappingProfile : Profile
{
    public ServiceCategoryMappingProfile()
    {
        CreateMap<ServiceCategory, ServiceCategoryDto>();
        CreateMap<ServiceCategoryDto, ServiceCategory>()
            .ForMember(dest => dest.Services, opt => opt.Ignore());
    }
} 