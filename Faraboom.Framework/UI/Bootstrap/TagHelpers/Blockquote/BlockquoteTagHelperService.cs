using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Blockquote
{
    [DataAnnotation.Injectable]
    public class BlockquoteTagHelperService : TagHelperService<BlockquoteTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("blockquote");
            output.TagName = "blockquote";
        }
    }
}
