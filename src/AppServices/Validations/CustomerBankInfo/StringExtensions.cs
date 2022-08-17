using System.Linq;

namespace AppServices.Validations.CustomerBankInfo;

public static class StringExtensions
{
    public static bool IsValidAccount(this string account)
    {
        return account.All(char.IsDigit);
    }
}
