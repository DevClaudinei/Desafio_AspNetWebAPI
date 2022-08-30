﻿using Application.Models.PortfolioProduct.Request;
using Application.Models.PortfolioProduct.Response;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface IPortfolioProductAppService
{
    IEnumerable<PortfolioProductResult> GetAllPortfolioProduct();
    PortfolioProductResult GetPortfolioProductById(long id);
    (bool isValid, string message) Invest(InvestmentRequest request, long customerId);
}