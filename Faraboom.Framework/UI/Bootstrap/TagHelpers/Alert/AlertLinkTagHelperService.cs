using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Alert
{
    [DataAnnotation.Injectable]
    public class AlertLinkTagHelperService : TagHelperService<AlertLinkTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("alert-link");
            output.Attributes.RemoveAll("frb-alert-link");
        }
    }
}