using Application.Models.Product.Request;
using System;
using System.Collections.Generic;

namespace Application.Models.Portfolio.Request;

public class CreatePortfolioRequest
{
    
    public virtual ICollection<CreateProductRequest> Products { get; set; }
    public Guid CustomerId { get; init; }
}
