using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Tab
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-tab-dropdown")]
    public class TabDropdownTagHelper : TagHelper<TabDropdownTagHelper, TabDropdownTagHelperService>
    {
        [HtmlAttributeName("frb-title")]
        public string Title { get; set; }

        public TabDropdownTagHelper(TabDropdownTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
