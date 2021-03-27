using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    [DataAnnotation.Injectable]
    public class DropdownDividerTagHelperService : TagHelperService<DropdownDividerTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("dropdown-divider");
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}