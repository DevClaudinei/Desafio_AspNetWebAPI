using System.Collections.Generic;

namespace DomainModels.Entities;

public class Portfolio
{
    public decimal TotalBalance { get; set; } // patrimônio da carteira somando todos os ativos
    public virtual ICollection<Product> Products { get; set; } // lista de produtos comprados
}
