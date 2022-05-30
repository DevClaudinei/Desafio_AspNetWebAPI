using AutoMapper;
using DomainModels;
using DomainModels.Models;

namespace AppServices;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerViewModel>();
        CreateMap<CustomerToUpdate, Customer>();
        CreateMap<CustomerToCreate, Customer>();
    }
}