using Application.Models.Product.Request;
using System;
using System.Collections.Generic;

namespace Application.Models.Portfolio.Request;

public class UpdatePortfolioRequest
{
    public Guid Id { get; set; }
    //public decimal TotalBalance { get; set; } // patrimônio da carteira somando todos os ativos
    public virtual ICollection<CreateProductRequest> Products { get; set; }
    public Guid CustomerId { get; init; }
}
