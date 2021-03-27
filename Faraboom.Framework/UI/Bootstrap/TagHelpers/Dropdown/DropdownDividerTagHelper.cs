using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-dropdown-divider")]
    public class DropdownDividerTagHelper : TagHelper<DropdownDividerTagHelper, DropdownDividerTagHelperService>
    {
        public DropdownDividerTagHelper(DropdownDividerTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
