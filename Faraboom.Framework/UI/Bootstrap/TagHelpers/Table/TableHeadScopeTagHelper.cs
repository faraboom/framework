using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Table
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-th")]
    public class TableHeadScopeTagHelper : TagHelper<TableHeadScopeTagHelper, TableHeadScopeTagHelperService>
    {
        [HtmlAttributeName("frb-scope")]
        public ThScope Scope { get; set; } = ThScope.Default;

        public TableHeadScopeTagHelper(TableHeadScopeTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
