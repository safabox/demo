using System;
using System.Linq;
using Gestion.Common.Domain.Seguridad;
using Gestion.Common.Utils;

namespace Gestion.API.Controllers
{
    static class QueryFilterHelper
    {
        internal static IQueryable<Rol> FilterQueryRol(IQueryable<Rol> query, FilterOption filter)
        {
            var propertyName = filter.PropertyName.ToLower();
            var value = filter.Value;

            switch (propertyName)
            {
                case "nombre":
                    {
                        return query.Where(x => x.Nombre.Contains(value));
                    }
                case "descripcion":
                    {
                        return query.Where(x => x.Descripcion.Contains(value));
                    }
            }
            return query;
        }

        internal static IQueryable<Usuario> FilterQueryUsuario(IQueryable<Usuario> query, FilterOption filter)
        {
            var propertyName = filter.PropertyName.ToLower();
            var value = filter.Value;

            switch (propertyName)
            {
                case "nombre":
                    {
                        return query.Where(x => x.Nombre.Contains(value));
                    }
                case "apellido":
                    {
                        return query.Where(x => x.Apellido.Contains(value));
                    }
                case "nombreusuario":
                    {
                        return query.Where(x => x.NombreUsuario.Contains(value));
                    }
            }
            return query;
        }
    }
}