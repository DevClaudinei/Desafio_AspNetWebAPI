﻿using System;
using System.Collections.Generic;

namespace DomainModels.Entities;

public class Product
{
    protected Product() { }

    public Product(
        Guid id,
        string symbol,
        //int quotes,
        decimal unitPrice
        //DateTime convertedAt
    )
    {
        Id = id;
        Symbol = symbol;
        //Quotes = quotes;
        UnitPrice = unitPrice;
        //ConvertedAt = convertedAt;
    }

    public Guid Id { get; set; }
    public string Symbol { get; set; } // nome do ativo
    //public int Quotes { get; set; } // quantidade de cotas
    public decimal UnitPrice { get; set; } // preço de cada cota de um ativo
    //public decimal NetValue { get; set; } // valor liquido total multiplicando Quotes pelo UnitPrice
    //public DateTime ConvertedAt { get; set; } // data da compra
    //public virtual ICollection<Portfolio> Portfolios { get; set; } // lista de produtos comprados
    public virtual ICollection<PortfolioProduct> PortfolioProducts { get; set; }
}
