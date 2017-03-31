using System;
using System.ComponentModel.DataAnnotations;

namespace Gestion.Common.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NullableDateTimeRangeValidationAttribute : ValidationAttribute
    {
        public string FromDateFieldName { get; set; }
        public NullableDateTimeRangeValidationAttribute(string fromDateFieldName)
        {
            this.FromDateFieldName = fromDateFieldName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Nullable<DateTime> toDate = (Nullable<DateTime>)value;
            Nullable<DateTime> fromDate = (Nullable<DateTime>)validationContext.ObjectType.GetProperty(this.FromDateFieldName).GetValue(validationContext.ObjectInstance, null);

            if (toDate.HasValue && fromDate.HasValue)
            {
                if (fromDate.Value.CompareTo(toDate.Value) > 0)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}

