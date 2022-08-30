namespace Application.Models.Response;

public class CustomerBankInfoResult
{
    public long Id { get; init; }
    public string Account { get; set; }
    public decimal AccountBalance { get; set; }
    public long CustomerId { get; init; }
}