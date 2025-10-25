using System.Security.Cryptography;

namespace CarDealership.util;

public static class RandomCodeGenerator
{
    public static string GenerateNumericCode(int digits = 6)
    {
        if (digits <= 0) digits = 6;
        var bytes = RandomNumberGenerator.GetBytes(digits);
        var chars = new char[digits];
        for (int i = 0; i < digits; i++)
        {
            chars[i] = (char)('0' + (bytes[i] % 10));
        }
        return new string(chars);
    }
}

