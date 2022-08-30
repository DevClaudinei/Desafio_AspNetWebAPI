using Application.Models.Portfolio.Response;
using Application.Models.PortfolioProduct.Request;
using Application.Models.PortfolioProduct.Response;
using AutoMapper;
using DomainModels.Entities;

namespace AppServices.Profiles;

public class PortfolioProductProfile : Profile
{
	public PortfolioProductProfile()
	{
        CreateMap<PortfolioProduct, PortfolioProductResult>();
        CreateMap<PortfolioResult, PortfolioProductResult>();
        CreateMap<UpdatePortfolioProductRequest, PortfolioProduct>();
        CreateMap<InvestmentRequest, PortfolioProduct>();

    }
}
