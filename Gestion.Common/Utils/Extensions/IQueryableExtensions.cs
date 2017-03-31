using System;
using System.Linq;
using System.Linq.Expressions;

namespace Gestion.Common.Utils.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> source, SortingOption option)
        {
            return source.OrderByPropertyName(option.PropertyName, option.Ascending);
        }

        public static IQueryable<T> OrderByPropertyName<T>(this IQueryable<T> source, string sortField, bool ascending = true)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, sortField);
            var exp = Expression.Lambda(prop, param);
            string method = ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { source.ElementType, exp.Body.Type };
            var rs = Expression.Call(typeof(Queryable), method, types, source.Expression, exp);
            return source.Provider.CreateQuery<T>(rs);
        }

        public static IQueryable<T> ThenByProperty<T>(this IQueryable<T> source, SortingOption option)
        {
            return source.ThenByPropertyName(option.PropertyName, option.Ascending);
        }

        public static IQueryable<T> ThenByPropertyName<T>(this IQueryable<T> source, string sortField, bool ascending = true)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, sortField);
            var exp = Expression.Lambda(prop, param);
            string method = ascending ? "ThenBy" : "ThenByDescending";
            Type[] types = new Type[] { source.ElementType, exp.Body.Type };
            var rs = Expression.Call(typeof(Queryable), method, types, source.Expression, exp);
            return source.Provider.CreateQuery<T>(rs);
        }

        public static IQueryable<T> OrderByProperties<T>(this IQueryable<T> source, SortingOption[] options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            var result = source;

            if (options.Length >= 0)
            {
                result = source.OrderByProperty(options[0]);

                for (int i = 1; i < options.Length; i++)
                {
                    result = result.ThenByProperty(options[i]);
                }
            }

            return result;
        }
    }
}
