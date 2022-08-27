using Application.Models;
using System.Collections.Generic;
using System;
using Application.Models.PortfolioProduct;

namespace AppServices.Services.Interfaces;

public interface IPortfolioProductAppService
{
    IEnumerable<PortfolioProductResult> GetAllPortfolioProduct();
    PortfolioProductResult GetPortfolioProductById(Guid id);
}
