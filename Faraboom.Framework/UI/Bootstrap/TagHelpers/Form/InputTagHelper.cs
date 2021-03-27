using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Form
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-input")]
    public class InputTagHelper : TagHelper<InputTagHelper, InputTagHelperService>
    {
        [HtmlAttributeName("frb-label")]
        public string Label { get; set; }

        [HtmlAttributeName("frb-info")]
        public string InfoText { get; set; }

        [HtmlAttributeName("frb-disabled")]
        public bool IsDisabled { get; set; } = false;

        [HtmlAttributeName("frb-readonly")]
        public bool? IsReadonly { get; set; } = false;

        [HtmlAttributeName("frb-auto-focus")]
        public bool AutoFocus { get; set; }

        [HtmlAttributeName("frb-type")]
        public string InputTypeName { get; set; }

        [HtmlAttributeName("frb-size")]
        public FormControlSize Size { get; set; } = FormControlSize.Default;

        [HtmlAttributeName("frb-format")]
        public string Format { get; set; }

        [HtmlAttributeName("frb-suppress-label")]
        public bool SuppressLabel { get; set; }

        public InputTagHelper(InputTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
