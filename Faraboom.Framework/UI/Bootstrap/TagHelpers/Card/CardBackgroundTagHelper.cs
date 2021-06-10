﻿namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Card
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-card", Attributes = "frb-background")]
    [HtmlTargetElement("frb-card-header", Attributes = "frb-background")]
    [HtmlTargetElement("frb-card-body", Attributes = "frb-background")]
    [HtmlTargetElement("frb-card-footer", Attributes = "frb-background")]
    public class CardBackgroundTagHelper : TagHelper<CardBackgroundTagHelper, CardBackgroundTagHelperService>
    {
        public CardBackgroundTagHelper(CardBackgroundTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("frb-background")]
        public CardBackgroundType Background { get; set; } = CardBackgroundType.Default;
    }
}
