using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Nav
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-nav-item")]
    public class NavItemTagHelper : TagHelper<NavItemTagHelper, NavItemTagHelperService>
    {
        [HtmlAttributeName("frb-dropdown")]
        public bool? Dropdown { get; set; }

        public NavItemTagHelper(NavItemTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
