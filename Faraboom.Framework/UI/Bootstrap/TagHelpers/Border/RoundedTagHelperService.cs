using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Border
{
    [DataAnnotation.Injectable]
    public class RoundedTagHelperService : TagHelperService<RoundedTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var roundedClass = "rounded";

            if (TagHelper.Rounded != RoundedType.Default)
            {
                roundedClass += "-" + TagHelper.Rounded.ToString().ToLowerInvariant().Replace("_","");
            }

            output.Attributes.AddClass(roundedClass);
        }
    }
}