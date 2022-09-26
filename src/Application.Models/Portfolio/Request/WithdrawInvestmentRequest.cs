using System;

namespace Application.Models.Portfolio.Request;

public class WithdrawInvestmentRequest
{
    public WithdrawInvestmentRequest(long portfolioId, long productId, int quotes, DateTime convertedAt)
    {
        PortfolioId = portfolioId;
        ProductId = productId;
        Quotes = quotes;
        ConvertedAt = convertedAt;
    }

    public long PortfolioId { get; set; }
    public long ProductId { get; set; }
    public int Quotes { get; set; }
    public DateTime ConvertedAt { get; set; }
}