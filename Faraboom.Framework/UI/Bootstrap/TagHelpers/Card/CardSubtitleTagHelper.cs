using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Card
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-card-subtitle")]
    public class CardSubtitleTagHelper : TagHelper<CardSubtitleTagHelper, CardSubtitleTagHelperService>
    {
        [HtmlAttributeName("frb-default-heading")]
        public static HtmlHeadingType DefaultHeading { get; set; } = HtmlHeadingType.H6;

        [HtmlAttributeName("frb-heading")]
        public HtmlHeadingType Heading { get; set; } = DefaultHeading;

        public CardSubtitleTagHelper(CardSubtitleTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}