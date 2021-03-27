using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Button
{
    [DataAnnotation.Injectable]
    public class ButtonToolbarTagHelperService : TagHelperService<ButtonToolbarTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("btn-toolbar");
            output.Attributes.Add("role","toolbar");
        }
    }
}
