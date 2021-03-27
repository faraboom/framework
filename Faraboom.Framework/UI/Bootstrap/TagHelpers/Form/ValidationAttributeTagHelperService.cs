using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Form
{
    [DataAnnotation.Injectable]
    public class ValidationAttributeTagHelperService : TagHelperService<ValidationAttributeTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("text-danger");
        }
    }
}