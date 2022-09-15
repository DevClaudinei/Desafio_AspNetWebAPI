namespace Application.Models;
public class CreateCustomerBankInfoRequest
{
    public CreateCustomerBankInfoRequest(string account, long customerId)
    {
        Account = account;
        CustomerId = customerId;
    }

    public string Account { get; set; }
    public long CustomerId { get; set; }
}