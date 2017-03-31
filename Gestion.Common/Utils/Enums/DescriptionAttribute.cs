using System;

namespace Gestion.Common.Utils.Enums
{
    /// <summary>Indicates that an enum value has a description.</summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DescriptionAttribute : System.Attribute
    {
        /// <summary>The description for the enum value.</summary>
        public string Description { get; set; }

        /// <summary>Constructs a new DescriptionAttribute.</summary>
        public DescriptionAttribute() { }

        /// <summary>Constructs a new DescriptionAttribute.</summary>
        /// <param name="description">The initial value of the Description property.</param>
        public DescriptionAttribute(string description)
        {
            this.Description = description;
        }

        /// <summary>Returns the Description property.</summary>
        /// <returns>The Description property.</returns>
        public override string ToString()
        {
            return this.Description;
        }
    }
}
