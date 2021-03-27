using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Form
{
    [HtmlTargetElement("frb-grid-view-column", ParentTag = "frb-grid-view-column")]
    public class GridViewColumnTagHelper : TagHelper<GridViewColumnTagHelper, GridViewColumnTagHelperService>
    {

        [HtmlAttributeName("frb-entity-type")]
        public string EntityType { get; set; }

        #region Constructors

        public GridViewColumnTagHelper(GridViewColumnTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }

        #endregion
    }
}
