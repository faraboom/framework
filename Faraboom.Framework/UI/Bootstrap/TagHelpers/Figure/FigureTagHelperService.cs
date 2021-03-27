using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Figure
{
    [DataAnnotation.Injectable]
    public class FigureTagHelperService : TagHelperService<FigureTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "figure";
            output.Attributes.AddClass("figure");
        }
    }
}