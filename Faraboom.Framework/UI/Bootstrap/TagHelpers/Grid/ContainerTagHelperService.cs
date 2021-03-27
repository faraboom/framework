using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Grid
{
    [DataAnnotation.Injectable]
    public class ContainerTagHelperService : TagHelperService<ContainerTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("container");
        }
    }
}