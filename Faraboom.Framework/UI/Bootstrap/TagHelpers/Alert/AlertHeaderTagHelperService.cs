using Microsoft.AspNetCore.Razor.TagHelpers;
namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Alert
{
    [DataAnnotation.Injectable]
    public class AlertHeaderTagHelperService : TagHelperService<AlertHeaderTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("alert-heading");
        }
    }
}