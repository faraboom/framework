using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Blockquote
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-blockquote", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class BlockquoteTagHelper : TagHelper<BlockquoteTagHelper, BlockquoteTagHelperService>
    {
        public BlockquoteTagHelper(BlockquoteTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
            
        }
    }
}
