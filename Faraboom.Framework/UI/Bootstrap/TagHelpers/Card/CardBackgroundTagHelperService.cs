using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Card
{
    [DataAnnotation.Injectable]
    public class CardBackgroundTagHelperService : TagHelperService<CardBackgroundTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            SetBackground(context, output);
        }

        protected virtual void SetBackground(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Background == CardBackgroundType.Default)
            {
                return;
            }

            output.Attributes.AddClass("bg-" + TagHelper.Background.ToString().ToLowerInvariant());
        }
    }
}