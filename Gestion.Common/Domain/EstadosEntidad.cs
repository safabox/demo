using Gestion.Common.Utils.Enums;

namespace Gestion.Common.Domain
{
    [DescriptiveEnumEnforcement(DescriptiveEnumEnforcementAttribute.EnforcementTypeEnum.DefaultToString)]
    public enum EstadosEntidad
    {
        Activa = 1,
        Eliminada = 2,
    }
}
