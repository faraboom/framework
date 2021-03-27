using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Utils
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement(Attributes = "frb-auto-focus")]
    public class AutoFocusTagHelper : TagHelper
    {
        [HtmlAttributeName("frb-auto-focus")]
        public bool AutoFocus { get; set; }

        public AutoFocusTagHelper(IOptions<MvcViewOptions> optionsAccessor)
            : base(optionsAccessor)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (AutoFocus)
            {
                output.Attributes.Add("data-auto-focus", "true");
            }
        }
    }
}