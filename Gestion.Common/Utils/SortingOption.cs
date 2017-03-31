using Gestion.Common.Utils.Extensions;
using System.Collections.Generic;

namespace Gestion.Common.Utils
{
    public class SortingOption
    {
        public static SortingOption[] Parse(string[] options)
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
                option.PropertyName = option.PropertyName.FirstCharToUpper();
                sortingOptions.Add(option);
            }
            return sortingOptions.ToArray();
        }

        public string PropertyName { get; set; }
        public bool Ascending { get; set; }

        public SortingOption()
        {
            this.Ascending = true;
        }
    }
}
