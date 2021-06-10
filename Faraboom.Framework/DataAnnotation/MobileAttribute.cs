namespace Faraboom.Framework.DataAnnotation
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Faraboom.Framework.Core.Extensions.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public sealed class MobileAttribute : ValidationAttribute, IClientModelValidator
    {
        private static readonly Regex Pattern = new("^(09)[0-9]{2}[0-9]{7}$", RegexOptions.Compiled);

        public override bool IsValid(object value)
        {
            return string.IsNullOrWhiteSpace(value?.ToString()) || Pattern.IsMatch(value.ToString());
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-mobile", FormatErrorMessage(context.ModelMetadata.GetDisplayName())));
        }
    }
}