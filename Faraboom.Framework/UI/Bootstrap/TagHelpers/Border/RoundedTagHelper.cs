using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Border
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement(Attributes = "frb-rounded")]
    public class RoundedTagHelper : TagHelper<RoundedTagHelper, RoundedTagHelperService>
    {
        [HtmlAttributeName("frb-rounded")]
        public RoundedType Rounded { get; set; } = RoundedType.Default;

        public RoundedTagHelper(RoundedTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
