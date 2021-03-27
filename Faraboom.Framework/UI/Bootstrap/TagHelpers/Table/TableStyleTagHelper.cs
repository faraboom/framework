using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Table
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-tr")]
    [HtmlTargetElement("frb-td")]
    public class TableStyleTagHelper : TagHelper<TableStyleTagHelper, TableStyleTagHelperService>
    {
        [HtmlAttributeName("frb-table-style")]
        public TableStyle TableStyle { get; set; } = TableStyle.Default;

        public TableStyleTagHelper(TableStyleTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
