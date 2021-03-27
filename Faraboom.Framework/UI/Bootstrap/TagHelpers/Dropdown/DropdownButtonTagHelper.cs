using Faraboom.Framework.UI.Bootstrap.TagHelpers.Button;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-dropdown-button")]
    public class DropdownButtonTagHelper : TagHelper<DropdownButtonTagHelper, DropdownButtonTagHelperService>
    {
        [HtmlAttributeName("frb-text")]
        public string Text { get; set; }

        [HtmlAttributeName("frb-size")]
        public ButtonSize Size { get; set; } = ButtonSize.Default;

        [HtmlAttributeName("frb-dropdown-style")]
        public DropdownStyle DropdownStyle { get; set; } = DropdownStyle.Single;

        [HtmlAttributeName("frb-button-type")]
        public ButtonType ButtonType { get; set; } = ButtonType.Default;

        [HtmlAttributeName("frb-icon")]
        public string Icon { get; set; }

        [HtmlAttributeName("frb-icon-type")]
        public FontIconType IconType { get; set; } = FontIconType.FontAwesome;

        [HtmlAttributeName("frb-link")]
        public bool? Link { get; set; }

        [HtmlAttributeName("frb-nva-link")]
        public bool? NavLink { get; set; }

        public DropdownButtonTagHelper(DropdownButtonTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
