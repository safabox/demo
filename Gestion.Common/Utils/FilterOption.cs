using System;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Common.Utils
{
    public class FilterOption
    {
        public static FilterOption[] Parse(string filters)
        {
            if (filters != null)
            {
                filters = filters.TrimEnd('|');

                if (!string.IsNullOrEmpty(filters))
                {
                    var values = filters.Split('|');
                    if (values.Length % 2 != 0)
                    {
                        throw new ArgumentException("Criterio de búsqueda inválido");
                    }

                    var result = new List<FilterOption>();
                    for (int i = 0; i < values.Length; i += 2)
                    {
                        result.Add(new FilterOption()
                        {
                            PropertyName = values[i],
                            Value = values[i + 1]
                        });
                    }
                    return result.ToArray();
                }
            }
            return new FilterOption[] { };
        }

        public string PropertyName { get; set; }
        public string Value { get; set; }

        public TEnum GetValueAsEnum<TEnum>() where TEnum : struct
        {
            return EnumUtils.Parse<TEnum>(this.Value);
        }

        public long GetValueAsInt64()
        {
            return Convert.ToInt64(this.Value);
        }

        public bool GetValueAsBoolean(string value)
        {
            return Convert.ToBoolean(this.Value);
        }

        public IEnumerable<long> GetValueAsInt64List()
        {
            return this.Value.Split(',').Select(x => Convert.ToInt64(x));
        }
    }
}
