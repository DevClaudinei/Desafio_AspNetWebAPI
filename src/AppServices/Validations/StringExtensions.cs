using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AppServices;
public static class StringExtensions
{
    public static bool IsValidDocument(this string document)
    {
        var firstDigitChecker = 0;
        for (int i = 0; i < document.Length - 2; i++)
        {
            firstDigitChecker += document.ToIntAt(i) * (10 - i);
        }
        firstDigitChecker = (firstDigitChecker * 10) % 11;
        if (firstDigitChecker is 10) firstDigitChecker = 0;

        var secondDigitChecker = 0;
        for (int i = 0; i < document.Length - 1; i++)
        {
            secondDigitChecker += document.ToIntAt(i) * (11 - i);
        }
        secondDigitChecker = (secondDigitChecker * 10) % 11;
        if (secondDigitChecker is 10) secondDigitChecker = 0;

        return firstDigitChecker.Equals(document.ToIntAt(^2)) && secondDigitChecker.Equals(document.ToIntAt(^1));
    }

    public static int ToIntAt(this string cpf, Index index)
    {
        var indexValue = index.IsFromEnd
            ? cpf.Length - index.Value
            : index.Value;

        return (int)char.GetNumericValue(cpf, indexValue);
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
        var ageCustomer = new DateTime(DateTime.Now.Subtract(birthdate).Ticks).Year - 1;
        return ageCustomer >= 18
            ? true
            : false;
    }

    public static bool validateFields(this string fields)
    {
        List<string> partsOfFields = new();
        var piecesOfFields = fields.Trim().Split(" ").ToList();
        var fieldsAreValid = validatesIfFieldsHaveInvalidCharacters(piecesOfFields);
        var countEmptySpaces = piecesOfFields.Where(x => x.Equals("")).ToList();

        if (piecesOfFields.Count > 1 && countEmptySpaces.Count == 0 && fieldsAreValid) return true;

        return default;
    }

    public static bool validatesIfFieldsHaveInvalidCharacters(this List<string> fields)
    {
        var fieldsAreValid = fields.Where(x => x != "").ToList();
        var result = true;

        foreach (var field in fieldsAreValid)
        {
            var x = field.ToLower().Where(x => x >= 'a' && x <= 'z').ToList();
            if (x.Count != field.Length) return false;
        }

        return result;
    }
}