using FluentValidation;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AppServices.Validations.Customer;

public static class StringExtensions
{
    public static bool IsValidDocument(this string document)
    {
        if (document.Length != 11) return false;

        var firstDigitChecker = 0;
        for (int i = 0; i < document.Length - 2; i++)
        {
            firstDigitChecker += document.ToIntAt(i) * (10 - i);
        }
        firstDigitChecker = firstDigitChecker * 10 % 11;
        if (firstDigitChecker is 10) firstDigitChecker = 0;

        var secondDigitChecker = 0;
        for (int i = 0; i < document.Length - 1; i++)
        {
            secondDigitChecker += document.ToIntAt(i) * (11 - i);
        }
        secondDigitChecker = secondDigitChecker * 10 % 11;
        if (secondDigitChecker is 10) secondDigitChecker = 0;

        return firstDigitChecker.Equals(document.ToIntAt(^2)) && secondDigitChecker.Equals(document.ToIntAt(^1));
    }

    private static int ToIntAt(this string cpf, Index index)
    {
        var indexValue = index.IsFromEnd
            ? cpf.Length - index.Value
            : index.Value;

        return (int)char.GetNumericValue(cpf, indexValue);
    }

    public static bool IsCellphone(this string Cellphone)
    {
        var expression = "^\\((?:[14689][1-9]|2[12478]|3[1234578]|5[1345]|7[134579])\\) (?:[2-8]|9[1-9])[0-9]{3}\\-[0-9]{4}$";
        return Regex.Match(Cellphone, expression).Success;
    }

    public static bool IsPostalCode(this string cep)
    {
        var expression = "([0-9]{5}-[0-9]{3})";
        return Regex.Match(cep, expression).Success;
    }

    public static bool HasReachedAdulthood(this DateTime dateOfBirth)
    {
        var ageCustomer = new DateTime(DateTime.Now.Subtract(dateOfBirth).Ticks).Year - 1;
        return ageCustomer >= 18;
    }

    public static bool ContainsEmptySpace(this string fields)
        => fields.Split(" ").Any(x => x == string.Empty);

    public static bool AnySymbolOrSpecialCharacter(this string value)
        => value.Replace(" ", string.Empty).Any(x => !char.IsLetter(x));

    public static bool HasAtLeastTwoCharactersForEachWord(this string fields)
        => !fields.Split(" ").Where(x => x.Length < 2).Any();
}