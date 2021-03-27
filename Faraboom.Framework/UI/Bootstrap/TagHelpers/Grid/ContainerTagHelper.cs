using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Grid
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-container")]
    public class ContainerTagHelper : TagHelper<ContainerTagHelper, ContainerTagHelperService>
    {
        public ContainerTagHelper(ContainerTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
