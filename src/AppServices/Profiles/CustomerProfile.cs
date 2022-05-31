using Application.Models;
using AutoMapper;
using DomainModels;

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