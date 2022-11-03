using Application.Models.CustomerBackInfo.Response;
using AutoMapper;
using DomainModels.Entities;

namespace AppServices.Profiles;

public class CustomerBankInfoProfile : Profile
{
    public CustomerBankInfoProfile()
    {
        CreateMap<CustomerBankInfo, CustomerBankInfoResult>();
    }
}