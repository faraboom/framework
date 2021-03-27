using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Modal
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-modal-body")]
    public class ModalBodyTagHelper : TagHelper<ModalBodyTagHelper, ModalBodyTagHelperService>
    {
        public ModalBodyTagHelper(ModalBodyTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}