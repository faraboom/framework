using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Button
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-button-toolbar")]
    public class ButtonToolbarTagHelper : TagHelper<ButtonToolbarTagHelper, ButtonToolbarTagHelperService>
    {
        public ButtonToolbarTagHelper(ButtonToolbarTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
