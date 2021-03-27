using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    [DataAnnotation.Injectable]
    public class DropdownItemTextTagHelperService : TagHelperService<DropdownItemTextTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("dropdown-item-text");
            output.TagName = "span";
        }
    }
}