using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OnlineShop.Common.Helpers
{
    public static class StringHelper
    {
        public static string pageActive = "";
        public static string ToUnsignString(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            input = input.Replace(".", "-");
            input = input.Replace(" ", "-");
            input = input.Replace(",", "-");
            input = input.Replace(";", "-");
            input = input.Replace(":", "-");
            input = input.Replace("  ", "-");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            while (str2.Contains("--"))
            {
                str2 = str2.Replace("--", "-");
            }
            return str2.ToLower();
        }
        public static string GetNameByEmail(string input)
        {
            try
            {
                int endIndex = input.IndexOf('@');
                return input.Substring(0, endIndex);
            }
            catch
            {
                return input;
            }
        }

        public static string RandomString(string prefix, int size, bool includeNumber = true, bool lowerCase = false)
        {
            Random rand = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string charsAndNumber = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string result = "";

            if (includeNumber)
            {
                result = prefix + "-" + new string(Enumerable.Repeat(charsAndNumber, size)
            .Select(s => s[rand.Next(s.Length)]).ToArray());
            }
            else
            {
                result = prefix + "-" + new string(Enumerable.Repeat(chars, size)
            .Select(s => s[rand.Next(s.Length)]).ToArray());
            }

            if (lowerCase)
                result = result.ToLower();
            return result;
        }
    }
}
