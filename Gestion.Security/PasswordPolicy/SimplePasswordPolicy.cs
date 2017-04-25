using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gestion.Security.PasswordPolicy
{
    public class SimplePasswordPolicy : IPasswordPolicy
    {
        public int MinLength { get; set; }
        public int MaxLenght { get; set; }
        public int MinUppercaseCount { get; set; }
        public int MinLowercaseCount { get; set; }

        public SimplePasswordPolicy(int minLength = 1, int maxLenght = 100, int minUppercaseCount = 0, int minLowercaseCount = 0)
        {
            this.MinLength = minLength;
            this.MaxLenght = maxLenght;
            this.MinUppercaseCount = minUppercaseCount;
            this.MinLowercaseCount = minLowercaseCount;
        }

        public bool IsValidPassword(string password)
        {
            IEnumerable<string> errors;

            return IsValidPassword(password, out errors);
        }

        public bool IsValidPassword(string password, out IEnumerable<string> errors)
        {
            var errorList = new List<string> { };

            if (password.Length < this.MinLength)
            {
                errorList.Add(string.Format("La clave debe tener al menos {0} caracteres", this.MinLength));
            }

            if (password.Length > this.MaxLenght)
            {
                errorList.Add(string.Format("La clave debe tener {0} caracteres como máximo", this.MaxLenght));
            }

            if (GetUppercaseCount(password) < this.MinUppercaseCount)
            {
                errorList.Add(string.Format("La clave debe tener al menos {0} mayúsculas", this.MinUppercaseCount));
            }

            if (GetLowercaseCount(password) < this.MinLowercaseCount)
            {
                errorList.Add(string.Format("La clave debe tener al menos {0} minúsculas", this.MinLowercaseCount));
            }

            errors = errorList;
            return errorList.Count == 0;
        }

        private int GetLowercaseCount(string password)
        {
            var pattern = "[a-zñ]";
            return GetMatchesCount(password, pattern);
        }

        private int GetUppercaseCount(string password)
        {
            var pattern = "[A-ZÑ]";
            return GetMatchesCount(password, pattern);
        }

        private int GetMatchesCount(string password, string pattern)
        {
            return Regex.Matches(password, pattern).Count;
        }
    }
}
