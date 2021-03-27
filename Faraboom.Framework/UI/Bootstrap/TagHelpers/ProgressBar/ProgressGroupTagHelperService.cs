using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.ProgressBar
{
    [DataAnnotation.Injectable]
    public class ProgressGroupTagHelperService : TagHelperService<ProgressGroupTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("progress");
            output.TagName = "div";
        }
    }
}