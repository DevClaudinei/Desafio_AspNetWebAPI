﻿using Application.Models.PortfolioProduct.Response;
using DomainModels.Entities;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface IPortfolioProductAppService
{
    long Create(PortfolioProduct investment);
    IEnumerable<PortfolioProductResult> GetAll();
    PortfolioProductResult GetById(long id);
}