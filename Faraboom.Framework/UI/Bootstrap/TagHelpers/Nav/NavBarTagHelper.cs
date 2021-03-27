using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Nav
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-navbar")]
    public class NavBarTagHelper : TagHelper<NavBarTagHelper, NavBarTagHelperService>
    {
        public NavbarSize Size { get; set; } = NavbarSize.Default;

        public NavbarStyle NavbarStyle { get; set; } = NavbarStyle.Default;

        public NavBarTagHelper(NavBarTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
