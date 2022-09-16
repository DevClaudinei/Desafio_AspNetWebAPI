using Application.Models.Portfolio.Request;
using Application.Models.Portfolio.Response;
using AutoMapper;
using DomainModels.Entities;

namespace AppServices.Profiles;

public class PortfolioProfile : Profile
{
	public PortfolioProfile()
	{
        CreateMap<Portfolio, PortfolioResult>();
        CreateMap<UpdatePortfolioRequest, Portfolio>();
        CreateMap<PortfolioResult, Portfolio>();
    }
}