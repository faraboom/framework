using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Card
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-card-footer")]
    public class CardFooterTagHelper : TagHelper<CardFooterTagHelper, CardFooterTagHelperService>
    {
        public CardFooterTagHelper(CardFooterTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
