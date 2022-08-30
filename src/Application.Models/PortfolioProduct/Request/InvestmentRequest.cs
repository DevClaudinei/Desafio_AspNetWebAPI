using System;

namespace Application.Models.PortfolioProduct.Request;

public record InvestmentRequest(
    long ProductId,
    long PortfolioId,
    int Quotes,
    DateTime? ConvertedAt
);
