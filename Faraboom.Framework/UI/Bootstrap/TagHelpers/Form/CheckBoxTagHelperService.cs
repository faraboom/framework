using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Form
{
    [DataAnnotation.Injectable]
    public class CheckBoxTagHelperService : TagHelperService<CheckBoxTagHelper>
    {
        public CheckBoxTagHelperService()
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var html = GetHtml(context, output);

            output.TagName = "div";
            output.Attributes.Clear();
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetHtmlContent(html);
        }

        protected virtual string GetHtml(TagHelperContext context, TagHelperOutput output)
        {
            var value = TagHelper.For.ModelExplorer.Model as bool?;
            var name = TagHelper.For.Name;
            var disabled = TagHelper.IsDisabled ? " disabled" : "";
            var triple = TagHelper.For.Metadata.IsNullableValueType ? " data-triple=\"triple\"" : "";
            var @checked = value.GetValueOrDefault() ? " checked=\"checked\"" : "";
            var @readonly = TagHelper.For.Metadata.IsNullableValueType && !value.GetValueOrDefault() ? " readonly=\"readonly\"" : "";
            var label = string.IsNullOrEmpty(TagHelper.Label) ? TagHelper.For.ModelExplorer.GetSimpleDisplayText() : TagHelper.Label;

            return "<div class=\"custom-control custom-checkbox\">" +
                            "<input type=\"checkbox\" value=\"true\" class=\"custom-control-input\" " + disabled + " " + triple + " id=\"" + name + "\" name=\"" + name + "\" " + @checked + " " + @readonly + ">" +
                            (value == false ? "<input name=\"name\" type=\"hidden\" value=\"false\" data-rel=\"" + name + "\"/>" : "") +
                            "<label class=\"custom-control-label\" for=\"" + name + "\">" + label + "</label>" +
                    "</div>";
        }
    }
}
