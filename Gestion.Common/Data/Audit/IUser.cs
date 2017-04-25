namespace Gestion.Common.Data.Audit
{
    public interface IUser
    {
        string UserName { get; }
        string Sid { get; }
        bool PasswordExpired { get; }
        bool ValidatePassword(string password);
        void ChangePassword(string newPassword);
    }
}
