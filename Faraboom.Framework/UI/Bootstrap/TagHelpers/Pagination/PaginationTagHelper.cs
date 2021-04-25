using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Pagination
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-paginator")]
    public class PaginationTagHelper : TagHelper<PaginationTagHelper, PaginationTagHelperService>
    {
        [HtmlAttributeName("frb-for")]
        public new PagerModel For { get; set; }

        [HtmlAttributeName("frb-show-info")]
        public bool? ShowInfo { get; set; }

        public PaginationTagHelper(PaginationTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
