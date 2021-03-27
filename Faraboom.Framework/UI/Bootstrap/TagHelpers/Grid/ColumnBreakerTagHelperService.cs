using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Grid
{
    [DataAnnotation.Injectable]
    public class ColumnBreakerTagHelperService : TagHelperService<ColumnBreakerTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("w-100");
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}