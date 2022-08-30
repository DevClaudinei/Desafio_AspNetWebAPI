﻿using Application.Models.PortfolioProduct.Response;
using System.Collections.Generic;

namespace Application.Models.Portfolio.Response;

public class PortfolioResult
{
    public long Id { get; init; }
    public decimal TotalBalance { get; set; } // patrimônio da carteira somando todos os ativos
    public IEnumerable<PortfolioProductResult> Products { get; set; } // lista de produtos comprados
    public long CustomerId { get; init; }
}
