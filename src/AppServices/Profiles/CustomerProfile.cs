using Application.Models;
using AutoMapper;
using DomainModels;

namespace AppServices.Profiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerResult>();
        CreateMap<UpdateCustomerRequest, Customer>();
        CreateMap<CreateCustomerRequest, Customer>();
    }
}