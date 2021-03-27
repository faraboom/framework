using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-dropdown-item")]
    public class DropdownItemTagHelper : TagHelper<DropdownItemTagHelper, DropdownItemTagHelperService>
    {
        public bool? Active { get; set; }

        public bool? Disabled { get; set; }

        public DropdownItemTagHelper(DropdownItemTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
