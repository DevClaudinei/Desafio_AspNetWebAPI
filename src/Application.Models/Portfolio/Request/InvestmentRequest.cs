using System;

namespace Application.Models.Portfolio.Request;

public record InvestmentRequest(
    long ProductId,
    long PortfolioId,
    int Quotes,
    DateTime ConvertedAt
);