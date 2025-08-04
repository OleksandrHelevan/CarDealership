using System;
using System.Text;

namespace CarDealership.config.decoder
{
    public class DealershipPasswordEncoder
    {
        public static string Encode(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("password не може бути порожнім");

            var plainTextBytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}