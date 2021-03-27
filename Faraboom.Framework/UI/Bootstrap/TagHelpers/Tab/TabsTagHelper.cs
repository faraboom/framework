using Faraboom.Framework.UI.Bootstrap.TagHelpers.Grid;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Tab
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-tabs")]
    public class TabsTagHelper : TagHelper<TabsTagHelper, TabsTagHelperService>
    {
        [HtmlAttributeName("frb-tab-style")]
        public TabStyle TabStyle { get; set; } = TabStyle.Tab;

        [HtmlAttributeName("frb-vertical-header-size")]
        public ColumnSize VerticalHeaderSize { get; set; } = ColumnSize._3;

        public TabsTagHelper(TabsTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
