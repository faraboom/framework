using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-dropdown-item-text")]
    public class DropdownItemTextTagHelper : TagHelper<DropdownItemTextTagHelper, DropdownItemTextTagHelperService>
    {
        public DropdownItemTextTagHelper(DropdownItemTextTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
