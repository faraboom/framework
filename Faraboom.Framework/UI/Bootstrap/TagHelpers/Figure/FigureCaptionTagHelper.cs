using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Figure
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-figure-caption")]
    public class FigureCaptionTagHelper : TagHelper<FigureCaptionTagHelper, FigureCaptionTagHelperService>
    {
        public FigureCaptionTagHelper(FigureCaptionTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
