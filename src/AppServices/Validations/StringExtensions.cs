using System;
using System.Text.RegularExpressions;

namespace AppServices;
public static class StringExtensions
{
    public static bool IsValidDocument(this string document)
    {
        var expression = "[0-9]{3}\\.?[0-9]{3}\\.?[0-9]{3}\\.?[0-9]{2}";
        return Regex.Match(document, expression).Success;
    }

    public static bool IsCellphone(this string celnum)
    {
        var expression = "^\\((?:[14689][1-9]|2[12478]|3[1234578]|5[1345]|7[134579])\\) (?:[2-8]|9[1-9])[0-9]{3}\\-[0-9]{4}$";
        return Regex.Match(celnum, expression).Success;
    }

    public static bool IsPostalCode(this string cep)
    {
        var expression = "([0-9]{5}-[0-9]{3})";
        return Regex.Match(cep, expression).Success;
    }

    public static bool IsReachedAdulthood(this DateTime birthdate)
    {
        var CurrentDate = DateTime.Now.Year;
        return (CurrentDate - birthdate.Year) >= 18;
    }
}