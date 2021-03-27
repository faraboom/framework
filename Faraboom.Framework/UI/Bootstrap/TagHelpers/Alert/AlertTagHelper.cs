using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Alert
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-alert", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AlertTagHelper : TagHelper<AlertTagHelper, AlertTagHelperService>
    {
        [HtmlAttributeName("frb-alert-type")]
        public AlertType AlertType { get; set; } = AlertType.Default;

        [HtmlAttributeName("frb-dismissible")]
        public bool? Dismissible { get; set; }

        public AlertTagHelper(AlertTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
