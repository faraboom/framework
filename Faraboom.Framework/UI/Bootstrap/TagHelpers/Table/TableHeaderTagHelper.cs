using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Table
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-table-header")]
    public class TableHeaderTagHelper : TagHelper<TableHeaderTagHelper, TableHeaderTagHelperService>
    {
        public TableHeaderTheme Theme { get; set; } = TableHeaderTheme.Default;
        
        public TableHeaderTagHelper(TableHeaderTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
