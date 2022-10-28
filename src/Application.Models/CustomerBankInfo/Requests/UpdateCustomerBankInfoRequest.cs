namespace Application.Models.CustomerBackInfo.Requests;

public class UpdateCustomerBankInfoRequest
{
    protected UpdateCustomerBankInfoRequest() { }

    public UpdateCustomerBankInfoRequest(decimal accountBalance)
    {
        AccountBalance = accountBalance;
    }

    public string Account { get; set; }
    public decimal AccountBalance { get; set; }
    public long CustomerId { get; set; }
}