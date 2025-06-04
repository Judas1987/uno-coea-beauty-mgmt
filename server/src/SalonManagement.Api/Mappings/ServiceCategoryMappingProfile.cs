using AutoMapper;
using SalonManagement.Api.Models.ServiceCategories;
using SalonManagement.Dal.Dtos;

namespace SalonManagement.Api.Mappings;

public class ServiceCategoryMappingProfile : Profile
{
    public ServiceCategoryMappingProfile()
    {
        // Request > DTO
        CreateMap<CreateServiceCategoryRequest, ServiceCategoryDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UpdateServiceCategoryRequest, ServiceCategoryDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // DTO > ViewModel
        CreateMap<ServiceCategoryDto, ServiceCategoryViewModel>();
    }
} 