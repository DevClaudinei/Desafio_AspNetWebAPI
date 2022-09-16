using System;

namespace Application.Models.Portfolio.Request;

public record CreateInvestmentRequest(
    long ProductId,
    long PortfolioId,
    int Quotes,
    DateTime? ConvertedAt
);