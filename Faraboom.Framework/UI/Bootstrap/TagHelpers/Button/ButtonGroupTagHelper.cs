﻿namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Button
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-button-group")]
    public class ButtonGroupTagHelper : TagHelper<ButtonGroupTagHelper, ButtonGroupTagHelperService>
    {
        public ButtonGroupTagHelper(ButtonGroupTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        [HtmlAttributeName("fab-direction")]
        public ButtonGroupDirection Direction { get; set; } = ButtonGroupDirection.Horizontal;

        [HtmlAttributeName("fab-size")]
        public ButtonGroupSize Size { get; set; } = ButtonGroupSize.Default;
    }
}
