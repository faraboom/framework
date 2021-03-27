using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.ListGroup
{
    [DataAnnotation.Injectable]
    public class ListGroupTagHelperService : TagHelperService<ListGroupTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";
            output.Attributes.AddClass("list-group");

            if (TagHelper.Flush ?? false)
            {
                output.Attributes.AddClass("list-group-flush");
            }
        }
    }
}