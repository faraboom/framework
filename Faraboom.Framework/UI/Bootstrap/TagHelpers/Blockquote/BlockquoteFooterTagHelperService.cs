using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Blockquote
{
    [DataAnnotation.Injectable]
    public class BlockquoteFooterTagHelperService : TagHelperService<BlockquoteFooterTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("blockquote-footer");
        }
        
    }
}