using Faraboom.Framework.Core.Extensions.Collections.Generic;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System.Collections.Generic;
using System.Linq;

namespace Faraboom.Framework.DataAnnotation
{
    public sealed class CreditCardAttribute : ValidationAttribute, IClientModelValidator
    {
        public override bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return true;

            var attribute = new System.ComponentModel.DataAnnotations.CreditCardAttribute();
            var lst = value as List<string>;
            if (lst != null)
                return lst.All(t => string.IsNullOrWhiteSpace(t) || attribute.IsValid(t));

            return string.IsNullOrWhiteSpace(value.ToString()) || attribute.IsValid(value);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-creditcard", FormatErrorMessage(context.ModelMetadata.GetDisplayName())));
        }
    }
}