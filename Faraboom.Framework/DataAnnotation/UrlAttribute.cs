namespace Faraboom.Framework.DataAnnotation
{
    using System;
    using System.Collections.Generic;
    using Faraboom.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed class UrlAttribute : ValidationAttribute, IClientModelValidator
    {
        public override bool IsValid(object value)
        {
            return string.IsNullOrWhiteSpace(value?.ToString()) || Uri.TryCreate(value.ToString(), UriKind.Absolute, out _);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-url", FormatErrorMessage(context.ModelMetadata.GetDisplayName())));
        }
    }
}