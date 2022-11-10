using Application.Models.Order;
using Application.Models.Portfolio.Request;
using AutoMapper;
using DomainModels.Entities;

namespace AppServices.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderResult>();
        CreateMap<InvestmentRequest, Order>();
        CreateMap<UninvestimentRequest, Order>();
        CreateMap<OrderResult, Order>();
    }
}