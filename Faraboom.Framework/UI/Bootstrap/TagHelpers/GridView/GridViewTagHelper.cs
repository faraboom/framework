using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Form
{
    [HtmlTargetElement("frb-grid-view", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class GridViewTagHelper : TagHelper<GridViewTagHelper, GridViewTagHelperService>
    {
        [HtmlAttributeName("frb-entity-type")]
        public string EntityType { get; set; }

        [HtmlAttributeName("frb-data-url")]
        public string DataUrl { get; set; }

        [HtmlAttributeName("frb-add-url")]
        public string AddUrl { get; set; }

        [HtmlAttributeName("frb-add-title")]
        public string AddTitle { get; set; }

        [HtmlAttributeName("frb-edit-url")]
        public string EditUrl { get; set; }

        [HtmlAttributeName("frb-edit-title")]
        public string EditTitle { get; set; }

        [HtmlAttributeName("frb-delete-url")]
        public string DeleteUrl { get; set; }

        [HtmlAttributeName("frb-custom-buttons")]
        public string CustomButtons { get; set; }

        [HtmlAttributeName("frb-auto-refresh")]
        public bool AutoRefresh { get; set; }

        [HtmlAttributeName("frb-formatters")]
        public string Formatters { get; set; }

        [HtmlAttributeName("frb-columns")]
        public string columns { get; set; }

        #region Constructors

        public GridViewTagHelper(GridViewTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }

        #endregion
    }
}
