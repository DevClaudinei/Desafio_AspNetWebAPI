namespace DomainModels.Entities;

public class CustomerBankInfo : BaseModel
{
    protected CustomerBankInfo() { }

    public CustomerBankInfo(string account, decimal accountBalance)
    {
        Account = account;
        AccountBalance = accountBalance;
    }

    public string Account { get; set; } // código da conta
    public decimal AccountBalance { get; set; } // saldo da conta
    public virtual Customer Customer { get; set; }
    public long CustomerId { get; set; }
}
