using AutoMapper;
using SalonManagement.Api.Models.Customers;
using SalonManagement.Dal.Dtos;
using SalonManagement.Dal.Entities;

namespace SalonManagement.Api.Mappings;

public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        // Entity <-> DTO
        CreateMap<Customer, CustomerDto>();
        CreateMap<CustomerDto, Customer>()
            .ForMember(dest => dest.Appointments, opt => opt.Ignore());

        // ViewModel <-> DTO
        CreateMap<CustomerDto, CustomerViewModel>()
            .ForMember(dest => dest.AvailableDiscountAmount, opt => opt.MapFrom(src => src.LoyaltyPoints * 0.01m));

        // Request > DTO
        CreateMap<CreateCustomerRequest, CustomerDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.LoyaltyPoints, opt => opt.MapFrom(_ => 0));

        CreateMap<UpdateCustomerRequest, CustomerDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.LoyaltyPoints, opt => opt.Ignore());
    }
}
