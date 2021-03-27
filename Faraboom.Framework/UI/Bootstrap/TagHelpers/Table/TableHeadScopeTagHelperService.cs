﻿using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Table
{
    [DataAnnotation.Injectable]
    public class TableHeadScopeTagHelperService : TagHelperService<TableHeadScopeTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            SetScope(context, output);
        }

        protected virtual void SetScope(TagHelperContext context, TagHelperOutput output)
        {
            switch (TagHelper.Scope)
            {
                case ThScope.Default:
                    return;
                case ThScope.Row:
                    output.Attributes.Add("scope", "row");
                    return;
                case ThScope.Column:
                    output.Attributes.Add("scope","col");
                    return;
            }
        }
    }
}