using System;
using System.Linq;


namespace Gestion.Common.Utils.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                throw new ArgumentException(input);
            }
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}
