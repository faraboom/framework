﻿using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Card
{
    [DataAnnotation.Injectable]
    public class CardImageTagHelperService : TagHelperService<CardImageTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass(TagHelper.Position.ToClassName());
            output.Attributes.RemoveAll("frb-card-image");
        }
    }
}