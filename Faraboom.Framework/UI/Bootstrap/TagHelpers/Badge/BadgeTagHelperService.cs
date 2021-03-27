using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Badge
{
    [DataAnnotation.Injectable]
    public class BadgeTagHelperService : TagHelperService<BadgeTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            SetBadgeClass(context, output);
            SetBadgeStyle(context, output);
        }

        protected virtual void SetBadgeStyle(TagHelperContext context, TagHelperOutput output)
        {
            var badgeType = GetBadgeType(context, output);

            if (badgeType != BadgeType.Default && badgeType != BadgeType._)
            {
                output.Attributes.AddClass("badge-" + badgeType.ToString().ToLowerInvariant());
            }
        }

        protected virtual void SetBadgeClass(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("badge");

            if (TagHelper.BadgePillType != BadgeType._)
            {
                output.Attributes.AddClass("badge-pill");
            }
        }

        protected virtual BadgeType GetBadgeType(TagHelperContext context, TagHelperOutput output)
        {
            return TagHelper.BadgeType != BadgeType._ ? TagHelper.BadgeType : TagHelper.BadgePillType;
        }
    }
}