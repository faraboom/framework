using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Card
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-card", Attributes = "frb-background")]
    [HtmlTargetElement("frb-card-header", Attributes = "frb-background")]
    [HtmlTargetElement("frb-card-body", Attributes = "frb-background")]
    [HtmlTargetElement("frb-card-footer", Attributes = "frb-background")]
    public class CardBackgroundTagHelper : TagHelper<CardBackgroundTagHelper, CardBackgroundTagHelperService>
    {
        [HtmlAttributeName("frb-background")]
        public CardBackgroundType Background { get; set; } = CardBackgroundType.Default;

        public CardBackgroundTagHelper(CardBackgroundTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
