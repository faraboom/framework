﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Faraboom.Framework.DataAnnotation;

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Form
{
    [Injectable]
    public class GridViewColumnTagHelperService : TagHelperService<GridViewColumnTagHelper>
    {
        private readonly HtmlEncoder htmlEncoder;
        private readonly IHtmlGenerator htmlGenerator;
        private readonly IServiceProvider serviceProvider;

        public GridViewColumnTagHelperService(
            HtmlEncoder htmlEncoder,
            IHtmlGenerator htmlGenerator,
            IServiceProvider serviceProvider)
        {
            this.htmlEncoder = htmlEncoder;
            this.htmlGenerator = htmlGenerator;
            this.serviceProvider = serviceProvider;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await output.GetChildContentAsync();
            output.SuppressOutput();
        }
    }
}