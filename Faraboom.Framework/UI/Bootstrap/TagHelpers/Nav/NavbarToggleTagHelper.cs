using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Nav
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-navbar-toggle")]
    public class NavbarToggleTagHelper : TagHelper<NavbarToggleTagHelper, NavbarToggleTagHelperService>
    {
        [HtmlAttributeName("frb-id")]
        public string Id { get; set; }

        public NavbarToggleTagHelper(NavbarToggleTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
