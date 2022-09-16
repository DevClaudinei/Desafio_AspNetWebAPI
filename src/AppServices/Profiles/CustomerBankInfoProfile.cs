using Application.Models;
using Application.Models.Response;
using AutoMapper;
using DomainModels.Entities;

namespace AppServices.Profiles;

public class CustomerBankInfoProfile : Profile
{
    public CustomerBankInfoProfile()
    {
        CreateMap<CustomerBankInfo, CustomerBankInfoResult>();
        CreateMap<CustomerBankInfoResult, CustomerBankInfo>();
        CreateMap<UpdateCustomerBankInfoRequest, CustomerBankInfo>();
        CreateMap<CreateCustomerBankInfoRequest, CustomerBankInfo>();
    }
}