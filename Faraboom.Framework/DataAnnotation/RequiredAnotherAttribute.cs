using Faraboom.Framework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace Faraboom.Framework.DataAnnotation
{
    public sealed class RequiredAnotherAttribute : ValidationAttribute
    {
        private string otherProperty { get; set; }
        private short? minCountOtherProperty { get; set; }
        private short? maxCountOtherProperty { get; set; }

        public RequiredAnotherAttribute(string otherProperty)
            : base(() => "ValidationError")
        {
            SetFields(otherProperty, null, null);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_RequiredAnother);
        }

        public RequiredAnotherAttribute(string otherProperty, short minCountOtherProperty, short maxCountOtherProperty)
            : base(() => "ValidationError")
        {
            SetFields(otherProperty, minCountOtherProperty, maxCountOtherProperty);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_RequiredAnotherList);
        }

        private void SetFields(string otherProperty, short? minCountOtherProperty, short? maxCountOtherProperty)
        {
            if (string.IsNullOrWhiteSpace(otherProperty))
                throw new ArgumentNullException(nameof(otherProperty));

            this.otherProperty = otherProperty;
            this.minCountOtherProperty = minCountOtherProperty ?? 0;
            this.maxCountOtherProperty = maxCountOtherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var firstComparable = value as IComparable;

            var otherPropertyInfo = validationContext.ObjectType.GetProperty(otherProperty);
            var otherValue = otherPropertyInfo?.GetValue(validationContext.ObjectInstance, null);

            ValidationResult result = null;
            if (firstComparable != null)
            {
                var ov = otherValue as IEnumerable<object>;
                if (otherValue == null
                    || (maxCountOtherProperty.HasValue && (ov.Count() < minCountOtherProperty || maxCountOtherProperty < ov.Count())))
                {
                    var displayName = Globals.GetLocalizedDisplayName(validationContext.ObjectType.GetProperty(validationContext.MemberName));
                    var otherDisplayName = Globals.GetLocalizedDisplayName(otherPropertyInfo);
                    result = new ValidationResult(FormatErrorMessage(displayName, otherDisplayName));
                }
            }

            return result;
        }

        public string FormatErrorMessage(string name, string otherName)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, otherName, minCountOtherProperty, maxCountOtherProperty);
        }

        public new string ErrorMessageResourceName { get; } = nameof(Resources.GlobalResource.Validation_RequiredAnother);

        public new Type ErrorMessageResourceType { get; } = typeof(Resources.GlobalResource);
    }
}
