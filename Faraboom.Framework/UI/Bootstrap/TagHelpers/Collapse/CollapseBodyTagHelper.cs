using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Collapse
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-collapse-body")]
    public class CollapseBodyTagHelper : TagHelper<CollapseBodyTagHelper, CollapseBodyTagHelperService>
    {
        [HtmlAttributeName("frb-id")]
        public string Id { get; set; }

        [HtmlAttributeName("frb-multi")]
        public bool? Multi { get; set; }

        [HtmlAttributeName("frb-show")]
        public bool? Show { get; set; }

        public CollapseBodyTagHelper(CollapseBodyTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
