﻿using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Faraboom.Framework.Core.Extensions;
using Faraboom.Framework.Resources;
using Faraboom.Framework.UI.Bootstrap.TagHelpers.Extensions;

using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Pagination
{
    [DataAnnotation.Injectable]
    public class PaginationTagHelperService : TagHelperService<PaginationTagHelper>
    {
        private readonly IHtmlGenerator _generator;
        private readonly HtmlEncoder _encoder;

        public PaginationTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder)
        {
            _generator = generator;
            _encoder = encoder;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.For.ShownItemsCount <= 0)
            {
                output.SuppressOutput();
            }

            ProcessMainTag(context, output);
            await SetContentAsHtmlAsync(context, output);
        }

        protected virtual async Task SetContentAsHtmlAsync(TagHelperContext context, TagHelperOutput output)
        {
            var html = new StringBuilder("");

            html.AppendLine(GetOpeningTags(context, output));
            html.AppendLine(await GetPreviousButtonAsync(context, output));
            html.AppendLine(await GetPagesAsync(context, output));
            html.AppendLine(await GetNextButton(context, output));
            html.AppendLine(GetClosingTags(context, output));

            output.Content.SetHtmlContent(html.ToString());
        }

        protected virtual void ProcessMainTag(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.AddClass("row");
            output.Attributes.AddClass("mt-3");
        }

        protected virtual async Task<string> GetPagesAsync(TagHelperContext context, TagHelperOutput output)
        {
            var pagesHtml = new StringBuilder("");

            foreach (var page in TagHelper.For.Pages)
            {
                pagesHtml.AppendLine(await GetPageAsync(context, output, page));
            }

            return pagesHtml.ToString();
        }

        protected virtual async Task<string> GetPageAsync(TagHelperContext context, TagHelperOutput output, PageItem page)
        {
            var pageHtml = new StringBuilder("");

            pageHtml.AppendLine("<li class=\"page-item " + (TagHelper.For.CurrentPage == page.Index ? "active" : "") + "\">");

            if (page.IsGap)
            {
                pageHtml.AppendLine("<span class=\"page-link gap\">…</span>");
            }
            else if (!page.IsGap && TagHelper.For.CurrentPage == page.Index)
            {
                pageHtml.AppendLine("                                 <span class=\"page-link\">\r\n" +
                                    "                                    " + page.Index + "\r\n" +
                                    "                                    <span class=\"sr-only\">(current)</span>\r\n" +
                                    "                                </span>");
            }
            else
            {
                pageHtml.AppendLine(await RenderAnchorTagHelperLinkHtmlAsync(context, output, page.Index.ToString(), page.Index.ToString()));
            }

            pageHtml.AppendLine("</li>");

            return pageHtml.ToString();
        }

        protected virtual async Task<string> GetPreviousButtonAsync(TagHelperContext context, TagHelperOutput output)
        {
            var localizationKey = "PagerPrevious";
            var currentPage = TagHelper.For.CurrentPage == 1
                ? TagHelper.For.CurrentPage.ToString()
                : (TagHelper.For.CurrentPage - 1).ToString();
            return
                "<li class=\"page-item " + (TagHelper.For.CurrentPage == 1 ? "disabled" : "") + "\">\r\n" +
                (await RenderAnchorTagHelperLinkHtmlAsync(context, output, currentPage, localizationKey)) + "                </li>";
        }

        protected virtual async Task<string> GetNextButton(TagHelperContext context, TagHelperOutput output)
        {
            var localizationKey = "PagerNext";
            var currentPage = (TagHelper.For.CurrentPage + 1).ToString();
            return
                "<li class=\"page-item " + (TagHelper.For.CurrentPage >= TagHelper.For.TotalPageCount ? "disabled" : "") + "\">\r\n" +
                (await RenderAnchorTagHelperLinkHtmlAsync(context, output, currentPage, localizationKey)) +
                "                </li>";
        }

        protected virtual async Task<string> RenderAnchorTagHelperLinkHtmlAsync(TagHelperContext context, TagHelperOutput output, string currentPage, string localizationKey)
        {
            var anchorTagHelper = GetAnchorTagHelper(currentPage, out var attributeList);

            var tagHelperOutput = await anchorTagHelper.ProcessAndGetOutputAsync(attributeList, context, "a", TagMode.StartTagAndEndTag);

            SetHrefAttribute(currentPage, attributeList);

            tagHelperOutput.Content.SetHtmlContent(UIResource.ResourceManager.GetString(localizationKey) ?? localizationKey);

            var renderedHtml = tagHelperOutput.Render(_encoder);

            return renderedHtml;
        }

        private AnchorTagHelper GetAnchorTagHelper(string currentPage, out TagHelperAttributeList attributeList)
        {
            var anchorTagHelper = new AnchorTagHelper(_generator)
            {
                Page = TagHelper.For.PageUrl,
                ViewContext = TagHelper.ViewContext
            };

            anchorTagHelper.RouteValues.Add("currentPage", currentPage);
            anchorTagHelper.RouteValues.Add("sort", TagHelper.For.Sort);

            attributeList = new TagHelperAttributeList
            {
                new TagHelperAttribute("tabindex", "-1"),
                new TagHelperAttribute("class", "page-link")
            };

            return anchorTagHelper;
        }

        protected virtual string GetOpeningTags(TagHelperContext context, TagHelperOutput output)
        {
            var pagerInfo = (TagHelper.ShowInfo ?? false) ?
                "    <div class=\"col-sm-12 col-md-5\"> " + string.Format(UIResource.PagerInfo, TagHelper.For.ShowingFrom, TagHelper.For.ShowingTo, TagHelper.For.TotalItemsCount) + "</div>\r\n"
                : "";

            return
                pagerInfo +
                "    <div class=\"col-sm-12 col-md-7\">\r\n" +
                "        <nav aria-label=\"Page navigation\">\r\n" +
                "            <ul class=\"pagination justify-content-end\">";
        }

        protected virtual string GetClosingTags(TagHelperContext context, TagHelperOutput output)
        {
            return
                "            </ul>\r\n" +
                "         </ nav>\r\n" +
                "    </div>\r\n";
        }

        protected virtual void SetHrefAttribute(string currentPage, TagHelperAttributeList attributeList)
        {
            var hrefAttribute = attributeList.FirstOrDefault(x => x.Name.Equals("href", StringComparison.OrdinalIgnoreCase));

            if (hrefAttribute != null)
            {
                var pageUrl = TagHelper.For.PageUrl;
                var routeValue = $"currentPage={currentPage}{(TagHelper.For.Sort.IsNullOrWhiteSpace() ? "" : "&sort=" + TagHelper.For.Sort)}";
                pageUrl += pageUrl.Contains("?") ? "&" + routeValue : "?" + routeValue;

                attributeList.Remove(hrefAttribute);
                attributeList.Add(new TagHelperAttribute("href", pageUrl, hrefAttribute.ValueStyle));
            }
        }
    }
}