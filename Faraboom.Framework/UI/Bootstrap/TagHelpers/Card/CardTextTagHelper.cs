using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Card
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-card-text")]
    public class CardTextTagHelper : TagHelper<CardTextTagHelper, CardTextTagHelperService>
    {
        public CardTextTagHelper(CardTextTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}