﻿using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Form
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-select2")]
    public class SelectTagHelper : TagHelper<SelectTagHelper, SelectTagHelperService>
    {
        [HtmlAttributeName("frb-label")]
        public string Label { get; set; }

        [HtmlAttributeName("frb-items")]
        public IEnumerable<SelectListItem> Items { get; set; }

        [HtmlAttributeName("frb-size")]
        public FormControlSize Size { get; set; } = FormControlSize.Default;

        [HtmlAttributeName("frb-info")]
        public string InfoText { get; set; }

        public SelectTagHelper(SelectTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}