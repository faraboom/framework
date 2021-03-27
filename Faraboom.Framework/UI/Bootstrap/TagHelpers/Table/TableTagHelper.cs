﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Table
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-table")]
    public class TableTagHelper : TagHelper<TableTagHelper, TableTagHelperService>
    {
        [HtmlAttributeName("frb-responsive")]
        public bool? Responsive { get; set; }

        [HtmlAttributeName("frb-responsive-sm")]
        public bool? ResponsiveSm { get; set; }

        [HtmlAttributeName("frb-responsive-md")]
        public bool? ResponsiveMd { get; set; }

        [HtmlAttributeName("frb-responsive-lg")]
        public bool? ResponsiveLg { get; set; }

        [HtmlAttributeName("frb-responsive-xl")]
        public bool? ResponsiveXl { get; set; }

        [HtmlAttributeName("frb-dark-theme")]
        public bool? DarkTheme { get; set; }

        [HtmlAttributeName("frb-striped-rows")]
        public bool? StripedRows { get; set; }

        [HtmlAttributeName("frb-hoverable-rows")]
        public bool? HoverableRows { get; set; }

        [HtmlAttributeName("frb-small")]
        public bool? Small { get; set; }

        [HtmlAttributeName("frb-border-style")]
        public TableBorderStyle BorderStyle { get; set; } = TableBorderStyle.Default;

        public TableTagHelper(TableTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
        : base(tagHelperService, optionsAccessor)
        {
        }
    }
}