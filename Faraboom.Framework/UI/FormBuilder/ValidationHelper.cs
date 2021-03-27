using System.Collections.Generic;
using System.Linq;
using System.Text;

using Faraboom.Framework.UI.FormBuilder.UnobtrusiveValidation;

namespace Faraboom.Framework.UI.FormBuilder
{
    public static class ValidationHelper
    {
        public static string AllValidationMessages(this IFormBuilderHtmlHelper helper, string modelName)
        {
            if (HasErrors(helper, modelName))
            {
                var message = string.Join(", ", helper.ViewData.ModelState[modelName].Errors.Select(e => e.ErrorMessage));
                return message;
            }
            return "";
        }

        public static bool HasErrors(this IFormBuilderHtmlHelper helper, string modelName)
        {
            return helper.ViewData.ModelState.ContainsKey(modelName) &&
                   helper.ViewData.ModelState[modelName].Errors.Any();
        }

        public static List<UnobtrusiveValidationRule> UnobtrusiveValidationRules = new List<UnobtrusiveValidationRule>
        {
            GetRulesFromAttributes
        };



        private static IEnumerable<ModelClientValidationRule> GetRulesFromAttributes(PropertyViewModel property)
        {
            return property.GetCustomAttributes()
                           .SelectMany(attribute => UnobtrusiveValidationAttributeRules,
                                       (attribute, rule) => rule(property, attribute))
                           .Where(r => r != null);
        }

        public static List<UnobtrusiveValidationAttributeRule> UnobtrusiveValidationAttributeRules = new List
            <UnobtrusiveValidationAttributeRule>()
        {
            RangeAttribteRule,
            RequiredAttributeRule,
            StringLengthAttributeRule,
            RegexAttributeRule
        };

        private static ModelClientValidationRule RangeAttribteRule(PropertyViewModel propertyViewModel, object attribute)
        {
            var a = attribute as RangeAttribute;
            return (a == null)
                       ? null
                       : new ModelClientValidationRangeRule(a.FormatErrorMessage(propertyViewModel.DisplayName), a.Minimum,
                                                            a.Maximum);
        }

        private static ModelClientValidationRule RequiredAttributeRule(PropertyViewModel propertyViewModel, object attribute)
        {
            var a = attribute as RequiredAttribute;
            return (a == null)
                       ? null
                       : new ModelClientValidationRequiredRule(a.FormatErrorMessage(propertyViewModel.DisplayName));
        }

        private static ModelClientValidationRule StringLengthAttributeRule(PropertyViewModel propertyViewModel, object attribute)
        {
            var a = attribute as StringLengthAttribute;
            return (a == null)
                       ? null
                       : new ModelClientValidationStringLengthRule(a.FormatErrorMessage(propertyViewModel.DisplayName),
                                                                   a.MinimumLength, a.MaximumLength);
        }

        private static ModelClientValidationRule RegexAttributeRule(PropertyViewModel propertyViewModel, object attribute)
        {
            var a = attribute as RegularExpressionAttribute;
            return (a == null)
                       ? null
                       : new ModelClientValidationRegexRule(a.FormatErrorMessage(propertyViewModel.DisplayName), a.Pattern);
        }

        public delegate IEnumerable<ModelClientValidationRule> UnobtrusiveValidationRule(PropertyViewModel property);

        public delegate ModelClientValidationRule UnobtrusiveValidationAttributeRule(PropertyViewModel property, object attribute);

        public static string UnobtrusiveValidation(IFormBuilderHtmlHelper helper, PropertyViewModel property)
        {
            var sb = new StringBuilder();

            var rules = UnobtrusiveValidationRules.SelectMany(r => r(property));

            if (rules.Any() == false) return "";

            sb.Append("data-val=\"true\" ");
            foreach (var rule in rules)
            {
                var prefix = string.Format(" data-val-{0}", rule.ValidationType);
                sb.AppendFormat(prefix + "=\"{0}\" ", rule.ErrorMessage);
                foreach (var parameter in rule.ValidationParameters)
                {
                    sb.AppendFormat(prefix + "-{0}=\"{1}\" ", parameter.Key, parameter.Value);
                }
            }

            return sb.ToString();
        }

        public static string UnobtrusiveValidation(this PropertyViewModel property)
        {
            var sb = new StringBuilder();

            var rules = UnobtrusiveValidationRules.SelectMany(r => r(property));

            if (rules.Any() == false) return "";

            sb.Append("data-val=\"true\" ");
            foreach (var rule in rules)
            {
                var prefix = string.Format(" data-val-{0}", rule.ValidationType);
                sb.AppendFormat(prefix + "=\"{0}\" ", rule.ErrorMessage);
                foreach (var parameter in rule.ValidationParameters)
                {
                    sb.AppendFormat(prefix + "-{0}=\"{1}\" ", parameter.Key, parameter.Value);
                }
            }

            return sb.ToString();
        }
    }
}