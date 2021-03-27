using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Nav
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-navbar-nav")]
    public class NavbarNavTagHelper : TagHelper<NavbarNavTagHelper, NavbarNavTagHelperService>
    {
        public NavbarNavTagHelper(NavbarNavTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
