namespace Faraboom.Framework.DataAnnotation
{
    using System.Collections.Generic;
    using Faraboom.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed class EmailAddressAttribute : ValidationAttribute, IClientModelValidator
    {
        public EmailAddressAttribute()
        {
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_EmailAddress);
        }

        public override bool IsValid(object value)
        {
            return string.IsNullOrWhiteSpace(value?.ToString()) || new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(value);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-email", FormatErrorMessage(context.ModelMetadata.GetDisplayName())));
        }
    }
}