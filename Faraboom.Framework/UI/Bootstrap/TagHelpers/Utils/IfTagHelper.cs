using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Utils
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement(Attributes = "frb-if")]
    public class IfTagHelper : TagHelper
    {
        [HtmlAttributeName("frb-if")]
        public bool Condition { get; set; }

        public IfTagHelper(IOptions<MvcViewOptions> optionsAccessor)
            : base(optionsAccessor)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!Condition)
            {
                output.SuppressOutput();
            }
        }
    }
}
