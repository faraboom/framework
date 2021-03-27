using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Figure
{
    [DataAnnotation.Injectable]
    public class FigureImageTagHelperService : TagHelperService<FigureImageTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("figure-img");
        }
    }
}