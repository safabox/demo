using Gestion.Common.Domain;

namespace Gestion.Common.Services
{
    public static class ServiceChangeCommentBuilder
    {
        private const string INSERT = "A";
        private const string UPDATE = "U";
        private const string DELETE = "D";
        private const string RECOVER = "R";

        private const string FIELD_SEPARATOR = "|";

        public static string GetCreateComment<TEntidad>(string comments = null) where TEntidad : Entidad
        {
            return GetComment<TEntidad>(INSERT, comments);
        }

        public static string GetUpdateComment<TEntidad>(TEntidad entity, string comments = null) where TEntidad : Entidad
        {
            return GetComment<TEntidad>(GetEntityAction(entity, UPDATE), comments);
        }

        public static string GetRecoverComment<TEntidad>(TEntidad entity, string comments = null) where TEntidad : Entidad
        {
            return GetComment<TEntidad>(GetEntityAction(entity, RECOVER), comments);
        }

        public static string GetDeleteComment<TEntidad>(TEntidad entity, string comments = null) where TEntidad : Entidad
        {
            return GetComment<TEntidad>(GetEntityAction(entity, DELETE), comments);
        }

        private static string GetEntityAction<TEntidad>(TEntidad entity, string action) where TEntidad : Entidad
        {
            if (entity != null)
            {
                return GetAction(entity.Id, action);
            }
            return action;
        }

        private static string GetAction(object entityId, string action)
        {
            return string.Concat(entityId, FIELD_SEPARATOR, action);
        }

        private static string GetComment<TEntidad>(string action, string comments = null)
        {
            var type = typeof(TEntidad).Name;

            return string.Concat(type, FIELD_SEPARATOR, action, FIELD_SEPARATOR, comments ?? string.Empty);
        }
    }
}
