using System.Collections.Generic;
using System.Security.Claims;

namespace Gestion.Common.Domain.Auth
{
    public interface IUser
    {
        string UserName { get; }
        string DisplayName { get; }
        string Sid { get; }
        bool PasswordExpired { get; }

        bool ValidatePassword(string password);

        void ChangePassword(string newPassword);

    }
}
