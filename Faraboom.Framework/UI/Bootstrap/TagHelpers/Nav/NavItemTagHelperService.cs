using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Nav
{
    [DataAnnotation.Injectable]
    public class NavItemTagHelperService : TagHelperService<NavItemTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "li";
            output.Attributes.AddClass("nav-item");

            SetDropdownClass(context, output);
        }

        protected virtual void SetDropdownClass(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Dropdown ?? false)
            {
                output.Attributes.AddClass("dropdown");
            }
        }
    }
}