namespace Faraboom.Framework.DataAnnotation
{
    using System.Collections.Generic;
    using System.Linq;
    using Faraboom.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed class PhoneAttribute : ValidationAttribute, IClientModelValidator
    {
        public override bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return true;
            }

            var attribute = new System.ComponentModel.DataAnnotations.PhoneAttribute();
            if (value is List<string> lst)
            {
                return lst.All(t => string.IsNullOrWhiteSpace(t) || attribute.IsValid(t));
            }

            return attribute.IsValid(value);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-phone", FormatErrorMessage(context.ModelMetadata.GetDisplayName())));
        }
    }
}