using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.ProgressBar
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-progress-group")]
    public class ProgressGroupTagHelper : TagHelper<ProgressGroupTagHelper, ProgressGroupTagHelperService>
    {
        public ProgressGroupTagHelper(ProgressGroupTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
