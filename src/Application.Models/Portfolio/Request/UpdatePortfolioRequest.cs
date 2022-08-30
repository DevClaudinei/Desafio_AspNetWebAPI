using System.Collections.Generic;

namespace Application.Models.Portfolio.Request;

public class UpdatePortfolioRequest
{
    public long Id { get; set; }
    public decimal TotalBalance { get; set; } // patrimônio da carteira somando todos os ativos
    public virtual IEnumerable<UpdatePortfolioRequest> Products { get; set; }
}
    