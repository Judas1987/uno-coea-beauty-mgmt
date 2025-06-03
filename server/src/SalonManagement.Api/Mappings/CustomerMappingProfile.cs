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
        CreateMap<CustomerDto, Customer>();

        // ViewModel <-> DTO
        CreateMap<CustomerDto, CustomerViewModel>();
        CreateMap<CustomerViewModel, CustomerDto>();

        // Request > DTO
        CreateMap<CreateCustomerRequest, CustomerDto>();
        CreateMap<UpdateCustomerRequest, CustomerDto>();
    }
}
