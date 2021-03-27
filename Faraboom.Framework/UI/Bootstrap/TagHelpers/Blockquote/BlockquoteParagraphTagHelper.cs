using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Blockquote
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("p", ParentTag = "frb-blockquote")]
    public class BlockquoteParagraphTagHelper : TagHelper<BlockquoteParagraphTagHelper, BlockquoteParagraphTagHelperService>
    {
        public BlockquoteParagraphTagHelper(BlockquoteParagraphTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
