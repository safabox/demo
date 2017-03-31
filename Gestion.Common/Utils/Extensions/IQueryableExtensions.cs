using System.Linq;
using System.Linq.Dynamic;

namespace Gestion.Common.Utils.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, params SortingOption[] options)
        {
            var expr = BuildSortExpression(options);
            return source.OrderBy(expr);
        }

        private static string BuildSortExpression(SortingOption[] options)
        {
            return string.Join(", ", options.Select(x => x.GetExpression()));
        }
    }
}
