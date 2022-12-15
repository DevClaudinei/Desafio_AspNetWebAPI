using Application.Models.Enum;
using System;

namespace Application.Models.Portfolio.Request;

public record UninvestimentRequest(
    long ProductId,
    long PortfolioId,
    int Quotes,
    DateTime ConvertedAt,
    OrderDirection Direction
);