using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Nav
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-nav-link")]
    public class NavLinkTagHelper : TagHelper<NavLinkTagHelper, NavLinkTagHelperService>
    {
        [HtmlAttributeName("frb-active")]
        public bool? Active { get; set; }

        [HtmlAttributeName("frb-disabled")]
        public bool? Disabled { get; set; }

        public NavLinkTagHelper(NavLinkTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
