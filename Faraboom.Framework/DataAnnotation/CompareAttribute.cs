namespace Faraboom.Framework.DataAnnotation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using Faraboom.Framework.Core;
    using Faraboom.Framework.Core.Extensions.Collections.Generic;
    using Faraboom.Framework.Resources;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed class CompareAttribute : System.ComponentModel.DataAnnotations.CompareAttribute, IClientModelValidator
    {
        public CompareAttribute(string otherProperty)
            : base(otherProperty)
        {
            ErrorMessageResourceType = typeof(GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_Compare);
        }

        public new string OtherPropertyDisplayName { get; internal set; }

        public Constants.OperandType OperandType { get; set; } = Constants.OperandType.Equals;

        public new Type ErrorMessageResourceType
        {
            get
            {
                return base.ErrorMessageResourceType;
            }

            private set
            {
                base.ErrorMessageResourceType = value;
            }
        }

        public new string ErrorMessageResourceName
        {
            get
            {
                return base.ErrorMessageResourceName;
            }

            private set
            {
                base.ErrorMessageResourceName = value;
            }
        }

        public new string ErrorMessage
        {
            get
            {
                return base.ErrorMessage;
            }

            internal set
            {
                base.ErrorMessage = value;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, OtherPropertyDisplayName ?? OtherProperty, EnumHelper.LocalizeEnum(OperandType));
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-equalto-other", OtherProperty));
            var key = string.Empty;
            switch (OperandType)
            {
                case Constants.OperandType.Equals:
                    key = "equalTo";
                    break;
                case Constants.OperandType.GreaterThan:
                    key = "greaterThan";
                    break;
                case Constants.OperandType.LessThan:
                    key = "lessThan";
                    break;
                case Constants.OperandType.GreaterThanOrEqual:
                    key = "greaterThanEqualTo";
                    break;
                case Constants.OperandType.LessThanOrEqual:
                    key = "lessThanEqualTo";
                    break;
                case Constants.OperandType.NotEquals:
                    key = "notEqualTo";
                    break;
            }

            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>($"data-val-{key}", FormatErrorMessage(context.ModelMetadata.GetDisplayName())));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            if (otherPropertyInfo == null)
            {
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, GlobalResource.Validation_Compare_UnknownProperty, OtherProperty));
            }

            ValidationResult result = null;

            if (value is IComparable firstComparable && otherPropertyInfo?.GetValue(validationContext.ObjectInstance, null) is IComparable otherComparable)
            {
                var compareValue = firstComparable.CompareTo(otherComparable);

                if (OtherPropertyDisplayName == null)
                {
                    OtherPropertyDisplayName = Globals.GetLocalizedDisplayName(otherPropertyInfo);
                }

                switch (OperandType)
                {
                    case Constants.OperandType.GreaterThan:
                        if (compareValue <= 0)
                        {
                            result = new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                        }

                        break;
                    case Constants.OperandType.LessThan:
                        if (compareValue >= 0)
                        {
                            result = new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                        }

                        break;
                    case Constants.OperandType.Equals:
                        if (compareValue != 0)
                        {
                            result = new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                        }

                        break;
                }
            }

            return result;
        }
    }
}
