using Faraboom.Framework.Core.Extensions.Collections.Generic;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System;
using System.Collections.Generic;

namespace Faraboom.Framework.DataAnnotation
{
    public sealed class RegularExpressionAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientModelValidator
    {
        public RegularExpressionAttribute(string pattern)
            : base(pattern)
        {
            ErrorMessageResourceType = typeof(Resources.GlobalResource);
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_StringLength);
        }

        public override bool IsValid(object value)
        {
            return string.IsNullOrWhiteSpace(value?.ToString()) || base.IsValid(value);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-regex", FormatErrorMessage(context.ModelMetadata.GetDisplayName())));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-regex-pattern", Pattern));
        }

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
    }
}