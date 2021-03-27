using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Modal
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-modal-header")]
    public class ModalHeaderTagHelper : TagHelper<ModalHeaderTagHelper, ModalHeaderTagHelperService>
    {
        [HtmlAttributeName("frb-title")]
        public string Title { get; set; }

        public ModalHeaderTagHelper(ModalHeaderTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}