using Gestion.Common;
using Gestion.Common.Domain.Auth;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Gestion.Security
{
    public interface IUserManager : IDisposable
    {
        IUser GetByUserName(string userName);

        Task<OperationResult> ChangeUserPasswordAsync(string userName, string oldPassword, string newPassword);

        IEnumerable<IPermission> GetPermissionsForUser(string userName);

        Task<OperationResult> InvalidateUserSession(string userName);

    }
}
