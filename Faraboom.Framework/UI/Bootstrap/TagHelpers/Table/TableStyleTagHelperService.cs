using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Table
{
    [DataAnnotation.Injectable]
    public class TableStyleTagHelperService : TagHelperService<TableStyleTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            SetStyle(context,output);
        }

        protected virtual void SetStyle(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.TableStyle != TableStyle.Default)
            {
                output.Attributes.AddClass("table-" + TagHelper.TableStyle.ToString().ToLowerInvariant());
            }
        }
    }
}