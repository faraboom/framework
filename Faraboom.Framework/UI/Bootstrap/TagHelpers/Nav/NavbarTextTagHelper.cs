﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Nav
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("span", Attributes = "frb-navbar-text")]
    [HtmlTargetElement("frb-navbar-text")]
    public class NavbarTextTagHelper : TagHelper<NavbarTextTagHelper, NavbarTextTagHelperService>
    {
        public NavbarTextTagHelper(NavbarTextTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {
        }
    }
}
