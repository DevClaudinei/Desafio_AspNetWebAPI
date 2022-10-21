using System;

namespace Application.Models.Order;

public class OrderResult
{
    public long Id { get; set; }
    public int Quotes { get; set; }
    public decimal NetValue { get; set; }
    public DateTime ConvertedAt { get; set; }
}
