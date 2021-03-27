using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Tab
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-tab-link", TagStructure = TagStructure.WithoutEndTag)]
    public class TabLinkTagHelper : TagHelper<TabLinkTagHelper, TabLinkTagHelperService>
    {
        [HtmlAttributeName("frb-title")]
        public string Title { get; set; }

        [HtmlAttributeName("frb-parent-dropdown-name")]
        public string ParentDropdownName { get; set; }

        [HtmlAttributeName("frb-href")]
        public string Href { get; set; }

        public TabLinkTagHelper(TabLinkTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
