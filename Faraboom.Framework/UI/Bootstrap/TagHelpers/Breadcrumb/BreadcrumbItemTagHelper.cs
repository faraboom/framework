using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Breadcrumb
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-bread-crumb-item")]
    public class BreadcrumbItemTagHelper : TagHelper<BreadcrumbItemTagHelper, BreadcrumbItemTagHelperService>
    {
        [HtmlAttributeName("frb-href")]
        public string Href { get; set; }

        [HtmlAttributeName("frb-title")]
        public string Title { get; set; }

        [HtmlAttributeName("frb-active")]
        public bool Active { get; set; }

        public BreadcrumbItemTagHelper(BreadcrumbItemTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
