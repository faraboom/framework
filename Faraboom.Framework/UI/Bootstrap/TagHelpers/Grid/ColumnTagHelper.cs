﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Grid
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-column")]
    public class ColumnTagHelper : TagHelper<ColumnTagHelper, ColumnTagHelperService>
    {
        [HtmlAttributeName("frb-size")]
        public ColumnSize Size { get; set; }

        [HtmlAttributeName("frb-size-sm")]
        public ColumnSize SizeSm { get; set; }

        [HtmlAttributeName("frb-size-md")]
        public ColumnSize SizeMd { get; set; }

        [HtmlAttributeName("frb-size-lg")]
        public ColumnSize SizeLg { get; set; }

        [HtmlAttributeName("frb-size-xl")]
        public ColumnSize SizeXl { get; set; }

        [HtmlAttributeName("frb-offset")]
        public ColumnSize Offset { get; set; }

        [HtmlAttributeName("frb-offset-sm")]
        public ColumnSize OffsetSm { get; set; }

        [HtmlAttributeName("frb-offset-md")]
        public ColumnSize OffsetMd { get; set; }

        [HtmlAttributeName("frb-offset-lg")]
        public ColumnSize OffsetLg { get; set; }

        [HtmlAttributeName("frb-offset-xl")]
        public ColumnSize OffsetXl { get; set; }

        [HtmlAttributeName("frb-column-order")]
        public ColumnOrder ColumnOrder { get; set; }

        [HtmlAttributeName("frb-vertical-align")]
        public VerticalAlign VerticalAlign { get; set; } = VerticalAlign.Default;

        public ColumnTagHelper(ColumnTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}