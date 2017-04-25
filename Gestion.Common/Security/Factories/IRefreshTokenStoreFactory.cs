using Gestion.Security.Stores;

namespace Gestion.Security.Factories
{
    public interface IRefreshTokenStoreFactory
    {
        IRefreshTokenStore Create();
    }
}
