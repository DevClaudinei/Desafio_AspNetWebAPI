using Application.Models.PortfolioProduct.Request;
using Application.Models.PortfolioProduct.Response;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface IPortfolioProductAppService
{
    IEnumerable<PortfolioProductResult> GetAllPortfolioProduct();
    PortfolioProductResult GetPortfolioProductById(long id);
    long Invest(InvestmentRequest request, long customerId);
}
