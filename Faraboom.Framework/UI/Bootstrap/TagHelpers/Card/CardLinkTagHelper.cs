using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Card
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("a", Attributes = "frb-card-link")]
    [HtmlTargetElement("frb-card-link")]
    public class CardLinkTagHelper : TagHelper<CardLinkTagHelper, CardLinkTagHelperService>
    {
        public CardLinkTagHelper(CardLinkTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
         
        }
    }
}