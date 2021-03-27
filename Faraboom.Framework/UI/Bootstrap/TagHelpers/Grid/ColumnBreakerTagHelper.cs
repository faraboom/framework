﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Grid
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-column-breaker")]
    public class ColumnBreakerTagHelper : TagHelper<ColumnBreakerTagHelper, ColumnBreakerTagHelperService>
    {
        public ColumnBreakerTagHelper(ColumnBreakerTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
