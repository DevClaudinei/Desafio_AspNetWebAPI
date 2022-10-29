namespace Application.Models.CustomerBackInfo.Requests;

public class UpdateCustomerBankInfoRequest
{
    protected UpdateCustomerBankInfoRequest() { }

    public UpdateCustomerBankInfoRequest(
        string account,
        decimal accountBalance,
        long customerId
    )
    {
        Account = account;
        AccountBalance = accountBalance;
        CustomerId = customerId;
    }

    public string Account { get; set; }
    public decimal AccountBalance { get; set; }
    public long CustomerId { get; set; }
}