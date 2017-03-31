using System;

namespace Gestion.Common.Utils.Enums
{
    /// <summary>Indicates whether or not an enum must have a NameAttribute and a DescriptionAttribute.</summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class DescriptiveEnumEnforcementAttribute : System.Attribute
    {
        /// <summary>Defines the different types of enforcement for DescriptiveEnums.</summary>
        public enum EnforcementTypeEnum
        {
            /// <summary>Indicates that the enum must have a NameAttribute and a DescriptionAttribute.</summary>
            ThrowException,

            /// <summary>Indicates that the enum does not have a NameAttribute and a DescriptionAttribute, the value will be used instead.</summary>
            DefaultToString
        }

        /// <summary>The enforcement type for this DescriptiveEnumEnforcementAttribute.</summary>
        public EnforcementTypeEnum EnforcementType { get; set; }

        /// <summary>Constructs a new DescriptiveEnumEnforcementAttribute.</summary>
        public DescriptiveEnumEnforcementAttribute()
        {
            this.EnforcementType = EnforcementTypeEnum.DefaultToString;
        }

        /// <summary>Constructs a new DescriptiveEnumEnforcementAttribute.</summary>
        /// <param name="enforcementType">The initial value of the EnforcementType property.</param>
        public DescriptiveEnumEnforcementAttribute(EnforcementTypeEnum enforcementType)
        {
            this.EnforcementType = enforcementType;
        }
    }
}
