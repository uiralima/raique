using System;
using System.Security.Cryptography;
using System.Text;

namespace Raique.Library
{
    public static class StringUtilities
    {
        public static void ThrowIfIsNullOrEmpty(this string target, string objectName)
        {
            if (String.IsNullOrWhiteSpace(target))
            {
                throw new ArgumentException(objectName);
            }
        }
        public static string Hash(this string source)
        {
            //return _teste.Do();
            using (SHA256 hashAlgorithm = SHA256.Create())
            {
                byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(source));
                var sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
        public static string OnlyValidChars(this string source, string validChars)
        {
            StringBuilder result = new StringBuilder();
            foreach (var ch in source)
            {
                if (validChars.Contains(ch.ToString()))
                {
                    result.Append(ch);
                }
            }
            return result.ToString();
        }
        public static string OnlyNumbersStr(string source)
        {
            return OnlyValidChars(source, "0123456789");
        }
        public static string GenerateRandom(int length, string validChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ")
        {
            StringBuilder result = new StringBuilder();
            Random rnd = new Random();
            for (int i = 0; i < length; i++)
            {
                result.Append(validChars[rnd.Next(validChars.Length)]);
            }
            return result.ToString();
        }
        public static string FromDate(DateTime sourceDate)
        {
            return sourceDate.ToString("yyyyMMdd");
        }
        public static string FromDateTime(DateTime sourceDate)
        {
            return sourceDate.ToString("yyyyMMddHHmmss");
        }
    }
}
