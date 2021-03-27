using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Dropdown
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-dropdown")]
    public class DropdownTagHelper : TagHelper<DropdownTagHelper, DropdownTagHelperService>
    {
        public DropdownDirection Direction { get; set; } = DropdownDirection.Down;

        public DropdownTagHelper(DropdownTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
