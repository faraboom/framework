using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Alert
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("a", Attributes = "frb-alert-link", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AlertLinkTagHelper : TagHelper<AlertLinkTagHelper, AlertLinkTagHelperService>
    {
        public AlertLinkTagHelper(AlertLinkTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
