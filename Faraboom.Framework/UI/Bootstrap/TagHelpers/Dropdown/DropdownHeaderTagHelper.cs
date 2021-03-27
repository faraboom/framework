using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-dropdown-header")]
    public class DropdownHeaderTagHelper : TagHelper<DropdownHeaderTagHelper, DropdownHeaderTagHelperService>
    {
        public DropdownHeaderTagHelper(DropdownHeaderTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
