using Application.Models.Portfolio.Response;
using Application.Models.PortfolioProduct;
using AutoMapper;
using DomainModels.Entities;

namespace AppServices.Profiles;

public class PortfolioProductProfile : Profile
{
	public PortfolioProductProfile()
	{
        CreateMap<PortfolioProduct, PortfolioProductResult>().ReverseMap();
        CreateMap<PortfolioResult, PortfolioProductResult>().ReverseMap();
        CreateMap<UpdatePortfolioProductRequest, PortfolioProduct>().ReverseMap();
    }
}
