﻿using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Card
{
    [DataAnnotation.Injectable]
    public class CardTextColorTagHelperService : TagHelperService<CardTextColorTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            SetTextColor(context, output);
        }

        protected virtual void SetTextColor(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.TextColor == CardTextColorType.Default)
            {
                return;
            }

            output.Attributes.AddClass("text-" + TagHelper.TextColor.ToString().ToLowerInvariant());
        }
    }
}