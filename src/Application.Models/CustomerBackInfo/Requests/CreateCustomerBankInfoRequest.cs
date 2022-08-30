namespace Application.Models;
public class CreateCustomerBankInfoRequest
{
    public CreateCustomerBankInfoRequest(string account, decimal accountBalance, long customerId)
    {
        Account = account;
        AccountBalance = accountBalance;
        CustomerId = customerId;
    }
    public string Account { get; set; }
    public decimal AccountBalance { get; set; }
    public long CustomerId { get; set; }
}