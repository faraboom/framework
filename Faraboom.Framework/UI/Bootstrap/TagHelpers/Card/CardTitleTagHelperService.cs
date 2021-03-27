using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Card
{
    [DataAnnotation.Injectable]
    public class CardTitleTagHelperService : TagHelperService<CardTitleTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = TagHelper.Heading.ToHtmlTag();
            output.Attributes.AddClass("card-title");
        }
    }
}