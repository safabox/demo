using System;
using System.Collections.Generic;
using System.Linq;
using Gestion.Common.Utils.Enums;
using System.Reflection;

namespace Gestion.Common.Utils
{
    public static class EnumUtils
    {
        #region GetDescription

        /// <summary>Returns the description of the specified enum.</summary>
        /// <param name="value">The value of the enum for which to return the description.</param>
        /// <returns>A description of the enum, or the enum name if no description exists.</returns>
        public static string GetDescription(this Enum value)
        {
            return GetEnumDescription(value);
        }
        /// <summary>Returns the description of the specified enum.</summary>
        /// <param name="value">The value of the enum for which to return the description.</param>
        /// <returns>A description of the enum, or the enum name if no description exists.</returns>
        public static string GetDescription<TEnum>(object value) where TEnum : struct
        {
            return GetEnumDescription(value);
        }

        /// <summary>Returns the description of the specified enum.</summary>
        /// <param name="value">The value of the enum for which to return the description.</param>
        /// <returns>A description of the enum, or the enum name if no description exists.</returns>
        private static string GetEnumDescription(object value)
        {
            if (value == null)
            {
                return null;
            }

            Type type = value.GetType();
            // Make sure the object is an enum.
            if (!type.IsEnum)
            {
                throw new ArgumentException("Value parameter must be an enum.", "value");
            }

            var valueFieldInfo = type.GetField(value.ToString());
            if (valueFieldInfo != null)
            {
                var descriptionAttributes = valueFieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (descriptionAttributes != null)
                {
                    if (descriptionAttributes.Length == 1)
                    {
                        return descriptionAttributes[0].ToString();
                    }
                    else if (descriptionAttributes.Length > 1)
                    {
                        throw new ApplicationException("Too many Description attributes exist in enum of type '" + type.Name + "', value '" + value.ToString() + "'.");
                    }
                }

                bool throwExceptionEnforcementActive = IsThrowExceptionEnforcementActive(valueFieldInfo);
                if (throwExceptionEnforcementActive)
                {
                    throw new ApplicationException("No Description attributes exist in enforced enum of type '" + type.Name + "', value '" + value.ToString() + "'.");
                }
            }

            return value.ToString();
        }

        private static bool IsThrowExceptionEnforcementActive(FieldInfo valueFieldInfo)
        {
            var enforcementAttributes = valueFieldInfo.GetCustomAttributes(typeof(DescriptiveEnumEnforcementAttribute), false);
            if (enforcementAttributes != null && enforcementAttributes.Length == 1)
            {
                var attr = (DescriptiveEnumEnforcementAttribute)enforcementAttributes[0];
                return (attr.EnforcementType == DescriptiveEnumEnforcementAttribute.EnforcementTypeEnum.ThrowException);
            }
            return false;
        }

        #endregion

        #region GetList

        public static IEnumerable<TEnum> GetList<TEnum>() where TEnum : struct
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }

        public static IEnumerable<KeyValuePair<TEnum, string>> GetListWithDescription<TEnum>() where TEnum : struct
        {
            return GetListWithDescription<TEnum>(x => EnumUtils.GetDescription<TEnum>(x));
        }

        public static IEnumerable<KeyValuePair<TEnum, string>> GetListWithDescription<TEnum>(Func<TEnum, string> getDescription) where TEnum : struct
        {
            return GetList<TEnum>().Select(x => new KeyValuePair<TEnum, string>(x, getDescription.Invoke(x)));
        }

        #endregion

        #region Parse

        public static TEnum Parse<TEnum>(string value) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
                throw new NotSupportedException("TEnum must be an Enum");

            try
            {
                return (TEnum)Enum.Parse(typeof(TEnum), value);
            }
            catch
            {
                return default(TEnum);
            }
        }

        #endregion

    }
}
