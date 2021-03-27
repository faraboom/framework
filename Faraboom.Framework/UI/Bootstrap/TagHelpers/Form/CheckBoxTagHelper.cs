using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Form
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-checkbox")]
    public class CheckBoxTagHelper : TagHelper<CheckBoxTagHelper, CheckBoxTagHelperService>
    {
        [HtmlAttributeName("frb-label")]
        public string Label { get; set; }

        [HtmlAttributeName("frb-disabled")]
        public bool IsDisabled { get; set; } = false;

        public CheckBoxTagHelper(CheckBoxTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
