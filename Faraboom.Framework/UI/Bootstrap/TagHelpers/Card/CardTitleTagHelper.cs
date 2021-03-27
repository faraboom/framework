using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Card
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-card-title")]
    public class CardTitleTagHelper : TagHelper<CardTitleTagHelper, CardTitleTagHelperService>
    {
        [HtmlAttributeName("frb-default-heading")]
        public static HtmlHeadingType DefaultHeading { get; set; } = HtmlHeadingType.H5;

        [HtmlAttributeName("frb-heading")]
        public HtmlHeadingType Heading { get; set; } = DefaultHeading;

        public CardTitleTagHelper(CardTitleTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
