﻿using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Card
{
    [DataAnnotation.Injectable]
    public class CardTagHelperService : TagHelperService<CardTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("card");

            SetBorder(context, output);
        }
        protected virtual void SetBorder(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Border == CardBorderColorType.Default)
            {
                return;
            }

            output.Attributes.AddClass("border-" + TagHelper.Border.ToString().ToLowerInvariant());
        }
    }
}