using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Nav
{
    [DataAnnotation.Injectable]
    public class NavbarBrandTagHelperService : TagHelperService<NavbarBrandTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll("frb-navbar-brand");
            output.Attributes.AddClass("navbar-brand");
        }
    }
}