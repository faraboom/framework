using System.Collections.Generic;
using System.Linq;

using Faraboom.Framework.UI.FormBuilder.UnobtrusiveValidation;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Faraboom.Framework.UI.FormBuilder.AspNetCore
{
    public static class AspMvcValidationHelper
    {

        public static IHtmlContent AllValidationMessages(this IHtmlHelper helper, string modelName)
        {
            if (HasErrors(helper, modelName))
            {
                var message = string.Join(", ", helper.ViewData.ModelState[modelName].Errors.Select(e => e.ErrorMessage));
                return new HtmlString(message);
            }
            return new HtmlString("");
        }

        public static bool HasErrors(this IHtmlHelper helper, string modelName)
        {
            return helper.ViewData.ModelState.ContainsKey(modelName) &&
                   helper.ViewData.ModelState[modelName].Errors.Count > 0;
        }

        public static List<UnobtrusiveValidationRule> UnobtrusiveValidationRules = new List<UnobtrusiveValidationRule>
        {
            GetRulesFromAttributes
        };



        private static IEnumerable<ModelClientValidationRule> GetRulesFromAttributes(PropertyViewModel property)
        {
            return property.GetCustomAttributes()
                .SelectMany(t => UnobtrusiveValidationAttributeRules,
                            (attribute, rule) => rule(property, attribute))
                .Where(t => t != null);
        }

        public static List<UnobtrusiveValidationAttributeRule> UnobtrusiveValidationAttributeRules = new List
          <UnobtrusiveValidationAttributeRule>()
        {
            RangeAttribteRule,
            RequiredAttributeRule,
            StringLengthAttributeRule,
            RegexAttributeRule
        };

        private static ModelClientValidationRule RangeAttribteRule(PropertyViewModel propertyVm, object attribute)
        {
            var a = attribute as RangeAttribute;
            return (a == null) ? null : new ModelClientValidationRangeRule(a.FormatErrorMessage(propertyVm.DisplayName), a.Minimum, a.Maximum);
        }

        private static ModelClientValidationRule RequiredAttributeRule(PropertyViewModel propertyVm, object attribute)
        {
            var a = attribute as RequiredAttribute;
            return (a == null) ? null : new ModelClientValidationRequiredRule(a.FormatErrorMessage(propertyVm.DisplayName));
        }

        private static ModelClientValidationRule StringLengthAttributeRule(PropertyViewModel propertyVm, object attribute)
        {
            var a = attribute as StringLengthAttribute;
            return (a == null) ? null : new ModelClientValidationStringLengthRule(a.FormatErrorMessage(propertyVm.DisplayName), a.MinimumLength, a.MaximumLength);
        }
        private static ModelClientValidationRule RegexAttributeRule(PropertyViewModel propertyVm, object attribute)
        {
            var a = attribute as RegularExpressionAttribute;
            return (a == null) ? null : new ModelClientValidationRegexRule(a.FormatErrorMessage(propertyVm.DisplayName), a.Pattern);
        }

        public delegate IEnumerable<ModelClientValidationRule> UnobtrusiveValidationRule(PropertyViewModel property);
        public delegate ModelClientValidationRule UnobtrusiveValidationAttributeRule(PropertyViewModel property, object attribute);

        public static IHtmlContent UnobtrusiveValidation(this IHtmlHelper helper, PropertyViewModel property)
        {
            var unobtrusiveValidation = ValidationHelper.UnobtrusiveValidation(new AspNetCore.Wrappers.FormBuilderHtmlHelper(helper), property);

            return new HtmlString(unobtrusiveValidation);
        }
    }

}