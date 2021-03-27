using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Tab
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-tab")]
    public class TabTagHelper : TagHelper<TabTagHelper, TabTagHelperService>
    {
        [HtmlAttributeName("frb-title")]
        public string Title { get; set; }

        [HtmlAttributeName("frb-active")]
        public bool? Active { get; set; }

        [HtmlAttributeName("frb-parent-dropdown-name")]
        public string ParentDropdownName { get; set; }

        public TabTagHelper(TabTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
