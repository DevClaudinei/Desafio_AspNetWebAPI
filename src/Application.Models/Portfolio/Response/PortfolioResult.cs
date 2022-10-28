using Application.Models.Product.Response;
using System.Collections.Generic;

namespace Application.Models.Portfolio.Response;

public class PortfolioResult
{
    public PortfolioResult(long id, decimal totalBalance, IEnumerable<ProductResult> products, long customerId)
    {
        Id = id;
        TotalBalance = totalBalance;
        Products = products;
        CustomerId = customerId;
    }

    public long Id { get; init; }
    public decimal TotalBalance { get; set; }
    public IEnumerable<ProductResult> Products { get; set; }
    public long CustomerId { get; init; }
}