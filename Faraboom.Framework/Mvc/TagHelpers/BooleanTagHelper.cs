using Faraboom.Framework.UI.Bootstrap.TagHelpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

using System.Text;

namespace Faraboom.Framework.Mvc.TagHelpers
{
    [HtmlTargetElement("frb-boolean", TagStructure = TagStructure.WithoutEndTag)]
    public class BooleanTagHelper : UI.Bootstrap.TagHelpers.TagHelper
    {
        [HtmlAttributeName("frb-label")]
        public string Label { get; set; }

        public BooleanTagHelper(IOptions<MvcViewOptions> optionsAccessor)
            : base(optionsAccessor)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("form-check abc-checkbox abc-checkbox-success");
            output.TagMode = TagMode.StartTagAndEndTag;

            StringBuilder sb = new();

            var css = output.Attributes.GetClass() + " form-check-input styled";
            var val = Value as bool? ?? For.Model as bool?;
            var isNullable = For.Metadata.IsNullableValueType == true;
            sb.Append($"<input class='{css}' id='{ElementId}' name='{ElementName}' type='checkbox' value='true' {(val.GetValueOrDefault() ? "checked=checked" : "")} {(isNullable ? "data-triple=triple" : "")} {(isNullable && val.HasValue && !val.Value ? "readonly=readonly" : "")} />");
            sb.Append("<i>&nbsp;</i>");
            if (val == false)
                sb.Append($"<input name='{ElementName}' type='hidden' value='false' data-rel='{ElementId}' />");

            sb.Append(Label);

            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}