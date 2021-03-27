using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Blockquote
{
    [DataAnnotation.Injectable]
    public class BlockquoteParagraphTagHelperService : TagHelperService<BlockquoteParagraphTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("mb-0");
        }
        
    }
}