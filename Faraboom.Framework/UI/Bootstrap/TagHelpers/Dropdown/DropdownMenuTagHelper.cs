using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-dropdown-menu")]
    public class DropdownMenuTagHelper : TagHelper<DropdownMenuTagHelper, DropdownMenuTagHelperService>
    {
            public DropdownAlign Align { get; set; } = DropdownAlign.Left;

        public DropdownMenuTagHelper(DropdownMenuTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
