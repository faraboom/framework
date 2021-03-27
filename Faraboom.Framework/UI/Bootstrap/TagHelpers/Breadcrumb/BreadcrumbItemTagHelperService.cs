using System.Collections.Generic;
using System.Text.Encodings.Web;
using Faraboom.Framework.UI.Bootstrap.TagHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Breadcrumb
{
    [DataAnnotation.Injectable]
    public class BreadcrumbItemTagHelperService : TagHelperService<BreadcrumbItemTagHelper>
    {
        private readonly HtmlEncoder _encoder;

        public BreadcrumbItemTagHelperService(HtmlEncoder encoder)
        {
            _encoder = encoder;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "li";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.AddClass("breadcrumb-item");
            output.Attributes.AddClass(BreadcrumbItemActivePlaceholder);

            var list = context.GetValue<List<BreadcrumbItem>>(BreadcrumbItemsContent);

            output.Content.SetHtmlContent(GetInnerHtml(context, output));
            
            list.Add(new BreadcrumbItem
            {
                Html = output.Render(_encoder),
                Active = TagHelper.Active
            });

            output.SuppressOutput();
        }

        protected virtual string GetInnerHtml(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrWhiteSpace(TagHelper.Href))
            {
                output.Attributes.Add("aria-current", "page");
                return TagHelper.Title;
            }
            return "<a href=\"" + TagHelper.Href + "\">" + TagHelper.Title + "</a>";
        }

    }
}