using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Modal
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-modal-footer")]
    public class ModalFooterTagHelper : TagHelper<ModalFooterTagHelper, ModalFooterTagHelperService>
    {
        [HtmlAttributeName("frb-buttons")]
        public ModalButtons Buttons { get; set; }

        [HtmlAttributeName("frb-button-alignment")]
        public ButtonsAlign ButtonAlignment { get; set; } = ButtonsAlign.Default;

        public ModalFooterTagHelper(ModalFooterTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}