namespace Application.Models.CustomerBackInfo.Response;

public class CustomerBankInfoResult
{
    public CustomerBankInfoResult(
        long id,
        string account,
        decimal accountBalance,
        long customerId
    )
    {
        Id = id;
        Account = account;
        AccountBalance = accountBalance;
        CustomerId = customerId;
    }

    public long Id { get; init; }
    public string Account { get; set; }
    public decimal AccountBalance { get; set; }
    public long CustomerId { get; init; }
}