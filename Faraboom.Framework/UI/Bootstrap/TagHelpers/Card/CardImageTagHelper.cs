using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Card
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("img", Attributes = "frb-card-image", TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement("frb-image", Attributes = "frb-card-image", TagStructure = TagStructure.WithoutEndTag)]
    public class CardImageTagHelper : TagHelper<CardImageTagHelper, CardImageTagHelperService>
    {
        [HtmlAttributeName("frb-card-image")]
        public CardImagePosition Position { get; set; } = CardImagePosition.Top;

        public CardImageTagHelper(CardImageTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}