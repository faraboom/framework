using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Nav
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-nav")]
    public class NavTagHelper : TagHelper<NavTagHelper, NavTagHelperService>
    {
        [HtmlAttributeName("frb-align")]
        public NavAlign Align { get; set; } = NavAlign.Default;

        [HtmlAttributeName("frb-nam-style")]
        public NavStyle NavStyle { get; set; } = NavStyle.Default;

        public NavTagHelper(NavTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
