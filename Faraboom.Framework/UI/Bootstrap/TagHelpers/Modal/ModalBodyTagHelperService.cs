using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Modal
{
    [DataAnnotation.Injectable]
    public class ModalBodyTagHelperService : TagHelperService<ModalBodyTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("modal-body");
        }
    }
}