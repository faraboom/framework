using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Nav
{
    [DataAnnotation.Injectable]
    public class NavbarTextTagHelperService : TagHelperService<NavbarTextTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("navbar-text");
            output.Attributes.RemoveAll("frb-navbar-text");
        }
    }
}