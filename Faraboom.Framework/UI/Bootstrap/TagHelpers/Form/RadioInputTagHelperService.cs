namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Form
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Faraboom.Framework.UI.Bootstrap.TagHelpers.Extensions;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [DataAnnotation.Injectable]
    public class RadioInputTagHelperService : TagHelperService<RadioInputTagHelper>
    {
        public RadioInputTagHelperService()
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var selectItems = GetSelectItems(context, output);
            SetSelectedValue(context, output, selectItems);

            var order = TagHelper.For.ModelExplorer.GetDisplayOrder();

            var html = GetHtml(context, output, selectItems);

            AddGroupToFormGroupContents(context, TagHelper.For.Name, html, order, out var suppress);

            if (suppress)
            {
                output.SuppressOutput();
            }
            else
            {
                output.TagName = "div";
                output.Attributes.Clear();
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Content.SetHtmlContent(html);
            }
        }

        protected virtual string GetHtml(TagHelperContext context, TagHelperOutput output, IList<SelectListItem> selectItems)
        {
            var html = new StringBuilder(string.Empty);

            foreach (var selectItem in selectItems)
            {
                var inlineClass = (TagHelper.Inline ?? false) ? " custom-control-inline" : string.Empty;
                var id = TagHelper.For.Name + "Radio" + selectItem.Value;
                var name = TagHelper.For.Name;
                var selected = selectItem.Selected ? " checked=\"checked\"" : string.Empty;
                var disabled = (TagHelper.Disabled ?? false) ? " disabled" : string.Empty;

                var htmlPart = "<div class=\"custom-control custom-radio" + inlineClass + "\">\r\n" +
                               "  <input type=\"radio\" id=\"" + id + "\" name=\"" + name + "\" value=\"" + selectItem.Value + "\"" + selected + " class=\"custom-control-input\"" + disabled + ">\r\n" +
                               "  <label class=\"custom-control-label\" for=\"" + id + "\">" + selectItem.Text + "</label>\r\n" +
                               "</div>";

                html.AppendLine(htmlPart);
            }

            return html.ToString();
        }

        protected virtual IList<SelectListItem> GetSelectItems(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Items != null)
            {
                return TagHelper.Items.ToList();
            }

            if (TagHelper.For.ModelExplorer.Metadata.IsEnum)
            {
                return GetSelectItemsFromEnum(context, output, TagHelper.For.ModelExplorer);
            }

            var selectItemsAttribute = TagHelper.For.ModelExplorer.GetAttribute<SelectItemsAttribute>();
            if (selectItemsAttribute != null)
            {
                return GetSelectItemsFromAttribute(selectItemsAttribute, TagHelper.For.ModelExplorer);
            }

            throw new Exception("No items provided for select attribute.");
        }

        protected virtual IList<SelectListItem> GetSelectItemsFromEnum(TagHelperContext context, TagHelperOutput output, ModelExplorer explorer)
        {
            var selectItems = explorer.Metadata.IsEnum ? explorer.ModelType.GetTypeInfo().GetMembers(BindingFlags.Public | BindingFlags.Static)
                .Select((t, i) => new SelectListItem { Value = i.ToString(), Text = t.Name }).ToList() : null;

            return selectItems;
        }

        protected virtual IList<SelectListItem> GetSelectItemsFromAttribute(
            SelectItemsAttribute selectItemsAttribute,
            ModelExplorer explorer)
        {
            var selectItems = selectItemsAttribute.GetItems(explorer)?.ToList();

            if (selectItems == null)
            {
                return new List<SelectListItem>();
            }

            return selectItems;
        }

        protected virtual void SetSelectedValue(TagHelperContext context, TagHelperOutput output, IList<SelectListItem> selectItems)
        {
            var selectedValue = GetSelectedValue(context, output);

            if (!selectItems.Any(si => si.Selected))
            {
                var itemToBeSelected = selectItems.FirstOrDefault(si => si.Value == selectedValue);

                if (itemToBeSelected != null)
                {
                    itemToBeSelected.Selected = true;
                }
            }
        }

        protected virtual string GetSelectedValue(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.For.ModelExplorer.Metadata.IsEnum)
            {
                var baseType = TagHelper.For.ModelExplorer.Model?.GetType().GetEnumUnderlyingType();

                if (baseType == null)
                {
                    return null;
                }

                var valueAsString = Convert.ChangeType(TagHelper.For.ModelExplorer.Model, baseType);
                return valueAsString != null ? valueAsString.ToString() : string.Empty;
            }

            return TagHelper.For.ModelExplorer.Model?.ToString();
        }

        protected virtual void AddGroupToFormGroupContents(TagHelperContext context, string propertyName, string html, int order, out bool suppress)
        {
            var list = context.GetValue<List<FormGroupItem>>(FormGroupContents) ?? new List<FormGroupItem>();
            suppress = list == null;

            if (list != null && !list.Any(igc => igc.HtmlContent.Contains("id=\"" + propertyName.Replace('.', '_') + "\"")))
            {
                list.Add(new FormGroupItem
                {
                    HtmlContent = html,
                    Order = order,
                    PropertyName = propertyName,
                });
            }
        }
    }
}
