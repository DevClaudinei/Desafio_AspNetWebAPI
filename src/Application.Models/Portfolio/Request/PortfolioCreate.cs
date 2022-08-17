using Application.Models.Product.Request;
using System;
using System.Collections.Generic;

namespace Application.Models.Portfolio.Request;

public class PortfolioCreate
{
    public Guid CustomerId { get; init; }
    public virtual ICollection<CreateProductRequest> Products { get; init; }
}
