using System.Collections.Generic;

namespace Gestion.Security.PasswordPolicy
{
    public interface IPasswordPolicy
    {
        bool IsValidPassword(string password);
        bool IsValidPassword(string password, out IEnumerable<string> errors);
    }
}
