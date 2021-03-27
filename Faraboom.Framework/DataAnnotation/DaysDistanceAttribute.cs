using DynamicExpresso;
using Faraboom.Framework.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace Faraboom.Framework.DataAnnotation
{
    public sealed class DaysDistanceAttribute : ValidationAttribute
    {
        public string OtherProperty { get; set; }

        public int MaxDistance { get; set; }

        public string Expression { get; set; }

        public DaysDistanceAttribute(string otherProperty, int maxDistance, string expression = null)
            : base(() => "ValidationError")
        {
            OtherProperty = otherProperty;
            MaxDistance = maxDistance;
            Expression = expression;

            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_DaysDistance);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(Expression))
            {
                var properties = validationContext.ObjectType.GetProperties();
                var interpreter = new Interpreter();
                MaxDistance = interpreter.Eval<int>(Expression, (from info in properties
                                                                 where Expression.Contains(info.Name)
                                                                 select
                                                                 new Parameter(info.Name, info.PropertyType, info.GetValue(validationContext.ObjectInstance, null)))
                    .ToArray());
            }

            var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            var otherValue = (DateTime)otherPropertyInfo?.GetValue(validationContext.ObjectInstance, null);

            var displayName = Globals.GetLocalizedDisplayName(validationContext.ObjectType.GetProperty(validationContext.MemberName));
            var otherDisplayName = Globals.GetLocalizedDisplayName(otherPropertyInfo);

            return ((DateTime)value - otherValue).TotalDays > MaxDistance
                ? new ValidationResult(FormatErrorMessage(displayName, otherDisplayName))
                : null;
        }

        public string FormatErrorMessage(string name, string otherName)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, otherName, MaxDistance);
        }
    }
}