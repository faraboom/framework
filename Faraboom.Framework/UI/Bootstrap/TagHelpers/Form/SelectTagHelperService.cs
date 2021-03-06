namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Form
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Faraboom.Framework.Core;
    using Faraboom.Framework.DataAnnotation;
    using Faraboom.Framework.UI.Bootstrap.TagHelpers.Extensions;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.TagHelpers;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.CodeAnalysis;

    [Injectable]
    public class SelectTagHelperService : TagHelperService<SelectTagHelper>
    {
        private readonly IHtmlGenerator generator;
        private readonly HtmlEncoder encoder;
        private IEnumerable<Attribute> cachedModelAttributes;

        public SelectTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder)
        {
            this.generator = generator;
            this.encoder = encoder;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            cachedModelAttributes = TagHelper.For.ModelExplorer.GetAttributes();

            var innerHtml = await GetFormInputGroupAsHtmlAsync(context, output);

            var order = TagHelper.For.ModelExplorer.GetDisplayOrder();

            AddGroupToFormGroupContents(context, TagHelper.For.Name, SurroundInnerHtmlAndGet(context, output, innerHtml), order, out var suppress);

            if (suppress)
            {
                output.SuppressOutput();
            }
            else
            {
                output.TagName = "div";
                LeaveOnlyGroupAttributes(context, output);
                output.Attributes.AddClass("form-group");
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Content.SetHtmlContent(innerHtml);
            }
        }

        protected virtual async Task<string> GetFormInputGroupAsHtmlAsync(TagHelperContext context, TagHelperOutput output)
        {
            var selectTag = await GetSelectTagAsync(context, output);
            var selectAsHtml = selectTag.Render(encoder);
            var label = await GetLabelAsHtmlAsync(context, output, selectTag);
            var validation = await GetValidationAsHtmlAsync(context, output, selectTag);
            var infoText = GetInfoAsHtml(context, output, selectTag);

            return label + Environment.NewLine + selectAsHtml + Environment.NewLine + infoText + Environment.NewLine + validation;
        }

        protected virtual string SurroundInnerHtmlAndGet(TagHelperContext context, TagHelperOutput output, string innerHtml)
        {
            return "<div class=\"form-group\">" + Environment.NewLine + innerHtml + Environment.NewLine + "</div>";
        }

        protected virtual async Task<TagHelperOutput> GetSelectTagAsync(TagHelperContext context, TagHelperOutput output)
        {
            var selectTagHelper = new Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper(generator)
            {
                For = TagHelper.For,
                Items = GetSelectItems(context, output),
                ViewContext = TagHelper.ViewContext,
            };

            var selectTagHelperOutput = await selectTagHelper.ProcessAndGetOutputAsync(GetInputAttributes(context, output), context, "select", TagMode.StartTagAndEndTag);

            selectTagHelperOutput.Attributes.AddClass("form-control");
            selectTagHelperOutput.Attributes.AddClass(GetSize(context, output));
            AddDisabledAttribute(selectTagHelperOutput);
            AddInfoTextId(selectTagHelperOutput);

            return selectTagHelperOutput;
        }

        protected virtual void AddDisabledAttribute(TagHelperOutput inputTagHelperOutput)
        {
            if (!inputTagHelperOutput.Attributes.ContainsName("disabled") && cachedModelAttributes?.GetAttribute<UIHintAttribute>()?.Disabled == true)
            {
                inputTagHelperOutput.Attributes.Add("disabled", string.Empty);
            }
        }

        protected virtual IReadOnlyList<SelectListItem> GetSelectItems(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Items != null)
            {
                return TagHelper.Items.ToList();
            }

            if (IsEnum())
            {
                return GetSelectItemsFromEnum(context, output, TagHelper.For.ModelExplorer);
            }

            var selectItemsAttribute = cachedModelAttributes?.GetAttribute<SelectItemsAttribute>();
            if (selectItemsAttribute != null)
            {
                return GetSelectItemsFromAttribute(selectItemsAttribute, TagHelper.For.ModelExplorer);
            }

            throw new Exception("No items provided for select attribute.");
        }

        protected virtual async Task<string> GetLabelAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput selectTag)
        {
            var uIHintAttribute = cachedModelAttributes?.GetAttribute<UIHintAttribute>();
            if (uIHintAttribute != null && uIHintAttribute.LabelPosition == LabelPosition.Hidden)
            {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(TagHelper.Label))
            {
                return "<label " + GetIdAttributeAsString(selectTag) + ">" + TagHelper.Label + "</label>" + GetRequiredSymbol(context, output);
            }

            return await GetLabelAsHtmlUsingTagHelperAsync(context, output) + GetRequiredSymbol(context, output);
        }

        protected virtual string GetRequiredSymbol(TagHelperContext context, TagHelperOutput output)
        {
            if (!TagHelper.DisplayRequiredSymbol)
            {
                return string.Empty;
            }

            return cachedModelAttributes?.GetAttribute<RequiredAttribute>() != null ? "<span> * </span>" : string.Empty;
        }

        protected virtual void AddInfoTextId(TagHelperOutput inputTagHelperOutput)
        {
            var idAttr = inputTagHelperOutput.Attributes.FirstOrDefault(a => a.Name == "id");
            if (idAttr == null)
            {
                return;
            }

            var attribute = cachedModelAttributes?.GetAttribute<DisplayAttribute>();
            if (attribute != null)
            {
                var description = Globals.GetLocalizedValueInternal(attribute, TagHelper.For.Name, Constants.ResourceKey.Description);
                if (!string.IsNullOrWhiteSpace(description))
                {
                    inputTagHelperOutput.Attributes.Add("aria-describedby", description);
                }
            }
        }

        protected virtual string GetInfoAsHtml(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag)
        {
            var text = string.Empty;
            if (!string.IsNullOrEmpty(TagHelper.InfoText))
            {
                text = TagHelper.InfoText;
            }
            else
            {
                var attribute = cachedModelAttributes?.GetAttribute<DisplayAttribute>();
                if (attribute != null)
                {
                    var description = Globals.GetLocalizedValueInternal(attribute, TagHelper.For.Name, Constants.ResourceKey.Description);
                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        text = description;
                    }
                }
            }

            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");
            return $"<small id=\"{idAttr?.Value}InfoText\" class=\"form-text text-muted\">{text}</small>";
        }

#pragma warning disable CA1002 // Do not expose generic lists
        protected virtual List<SelectListItem> GetSelectItemsFromEnum(TagHelperContext context, TagHelperOutput output, ModelExplorer explorer)
#pragma warning restore CA1002 // Do not expose generic lists
        {
            var selectItems = new List<SelectListItem>();
            var isNullableType = Nullable.GetUnderlyingType(explorer.ModelType) != null;
            var enumType = explorer.ModelType;

            if (isNullableType)
            {
                enumType = Nullable.GetUnderlyingType(explorer.ModelType);
                selectItems.Add(new SelectListItem());
            }

            if (explorer.Metadata.ElementType?.IsEnum == true)
            {
                enumType = explorer.Metadata.ElementType;
                selectItems.Add(new SelectListItem());
            }

            foreach (var enumValue in enumType.GetEnumValues())
            {
                selectItems.Add(new SelectListItem
                {
                    Value = enumValue.ToString(),
                    Text = EnumHelper.LocalizeEnum(enumValue),
                });
            }

            return selectItems;
        }

        protected virtual IReadOnlyList<SelectListItem> GetSelectItemsFromAttribute(SelectItemsAttribute selectItemsAttribute, ModelExplorer explorer)
        {
            var selectItems = selectItemsAttribute.GetItems(explorer)?.ToList();

            if (selectItems == null)
            {
                return new List<SelectListItem>();
            }

            return selectItems;
        }

        protected virtual async Task<string> GetLabelAsHtmlUsingTagHelperAsync(TagHelperContext context, TagHelperOutput output)
        {
            var labelTagHelper = new LabelTagHelper(generator)
            {
                For = TagHelper.For,
                ViewContext = TagHelper.ViewContext,
            };

            return await labelTagHelper.RenderAsync(new TagHelperAttributeList(), context, encoder, "label", TagMode.StartTagAndEndTag);
        }

        protected virtual async Task<string> GetValidationAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag)
        {
            var validationMessageTagHelper = new ValidationMessageTagHelper(generator)
            {
                For = TagHelper.For,
                ViewContext = TagHelper.ViewContext,
            };

            var attributeList = new TagHelperAttributeList { { "class", "text-danger" } };

            return await validationMessageTagHelper.RenderAsync(attributeList, context, encoder, "span", TagMode.StartTagAndEndTag);
        }

        protected virtual string GetSize(TagHelperContext context, TagHelperOutput output)
        {
            var attribute = cachedModelAttributes?.GetAttribute<UIHintAttribute>();
            if (attribute != null)
            {
                TagHelper.Size = attribute.Size;
            }

            switch (TagHelper.Size)
            {
                case FormControlSize.Small:
                    return "custom-select-sm";
                case FormControlSize.Medium:
                    return "custom-select-md";
                case FormControlSize.Large:
                    return "custom-select-lg";
                default:
                    return string.Empty;
            }
        }

        protected virtual TagHelperAttributeList GetInputAttributes(TagHelperContext context, TagHelperOutput output)
        {
            var groupPrefix = "group-";

            var tagHelperAttributes = output.Attributes.Where(a => !a.Name.StartsWith(groupPrefix)).ToList();
            var attrList = new TagHelperAttributeList();

            foreach (var tagHelperAttribute in tagHelperAttributes)
            {
                attrList.Add(tagHelperAttribute);
            }

            attrList.AddClass("custom-select");

            return attrList;
        }

        protected virtual void LeaveOnlyGroupAttributes(TagHelperContext context, TagHelperOutput output)
        {
            var groupPrefix = "group-";
            var tagHelperAttributes = output.Attributes.Where(a => a.Name.StartsWith(groupPrefix)).ToList();

            output.Attributes.Clear();

            foreach (var tagHelperAttribute in tagHelperAttributes)
            {
                var nameWithoutPrefix = tagHelperAttribute.Name.Substring(groupPrefix.Length);
                var newAttritube = new TagHelperAttribute(nameWithoutPrefix, tagHelperAttribute.Value);
                output.Attributes.Add(newAttritube);
            }
        }

        protected virtual string GetIdAttributeAsString(TagHelperOutput inputTag)
        {
            var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");

            return idAttr != null ? "for=\"" + idAttr.Value + "\"" : string.Empty;
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

        private bool IsEnum()
        {
            var value = TagHelper.For.Model;
            if (value != null)
            {
                var type = value.GetType();
                if (type.IsEnum)
                {
                    return true;
                }

                if (type == typeof(IEnumerable<>) && type.GetGenericArguments()[0].IsEnum)
                {
                    return true;
                }
            }

            return TagHelper.For.ModelExplorer.Metadata.IsEnum || TagHelper.For.ModelExplorer.Metadata.ElementType?.IsEnum == true;
        }
    }
}
