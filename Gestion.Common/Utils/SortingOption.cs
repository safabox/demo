using Gestion.Common.Utils.Extensions;
using System.Collections.Generic;

namespace Gestion.Common.Utils
{
    public class SortingOption
    {
        public static SortingOption[] Parse(string[] options, IDictionary<string, string> fieldMappings = null)
        {
            List<SortingOption> sortingOptions = new List<SortingOption>();

            foreach (string sortField in options)
            {
                var option = new SortingOption();
                option.Ascending = true;

                if (sortField.StartsWith("+"))
                {
                    option.PropertyName = sortField.TrimStart('+');
                }
                else if (sortField.StartsWith("-"))
                {
                    option.PropertyName = sortField.TrimStart('-');
                    option.Ascending = false;
                }
                else
                {
                    option.PropertyName = sortField;
                }
                option.PropertyName = MapPropertyName(option.PropertyName, fieldMappings).FirstCharToUpper(); ;
                sortingOptions.Add(option);
            }
            return sortingOptions.ToArray();
        }

        public string PropertyName { get; set; }
        public bool Ascending { get; set; }

        public string GetExpression()
        {
            var order = this.Ascending ? string.Empty : " descending";
            return string.Concat(this.PropertyName, order);
        }

        public SortingOption()
        {
            this.Ascending = true;
        }

        private static string MapPropertyName(string propertyName, IDictionary<string, string> mappings)
        {
            if (mappings != null)
            {
                var result = string.Empty;
                if (mappings.TryGetValue(propertyName, out result))
                {
                    return result;
                }
            }
            return propertyName;
        }
    }
}
