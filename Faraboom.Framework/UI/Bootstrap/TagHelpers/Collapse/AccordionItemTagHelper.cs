using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Collapse
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-accordion-item")]
    public class AccordionItemTagHelper : TagHelper<AccordionItemTagHelper, AccordionItemTagHelperService>
    {
        [HtmlAttributeName("frb-id")]
        public string Id { get; set; }

        [HtmlAttributeName("frb-title")]
        public string Title { get; set; }

        [HtmlAttributeName("frb-active")]
        public bool? Active { get; set; }

        public AccordionItemTagHelper(AccordionItemTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
