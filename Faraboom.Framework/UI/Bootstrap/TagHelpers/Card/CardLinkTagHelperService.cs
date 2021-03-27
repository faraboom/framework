using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Card
{
    [DataAnnotation.Injectable]
    public class CardLinkTagHelperService : TagHelperService<CardLinkTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("card-link");
            output.Attributes.RemoveAll("frb-card-link");
        }
    }
}