using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Border
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement(Attributes = "frb-border")]
    public class BorderTagHelper : TagHelper<BorderTagHelper, BorderTagHelperService>
    {
        [HtmlAttributeName("frb-border")]
        public BorderType Border { get; set; } = BorderType.Default;

        public BorderTagHelper(BorderTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
