﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Nav
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement(Attributes = "frb-navbar-brand")]
    public class NavbarBrandTagHelper : TagHelper<NavbarBrandTagHelper, NavbarBrandTagHelperService>
    {
        public NavbarBrandTagHelper(NavbarBrandTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
