using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Card
{
    [DataAnnotation.Injectable]
    public class CardTagHelper : TagHelper<CardTagHelper, CardTagHelperService>
    {
        public CardBorderColorType Border { get; set; } = CardBorderColorType.Default;

        public CardTagHelper(CardTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
