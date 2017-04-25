using Gestion.Security.Stores;

namespace Gestion.Security.Factories
{
    public interface IAudiencesStoreFactory
    {
        IAudiencesStore Create();
    }
}
