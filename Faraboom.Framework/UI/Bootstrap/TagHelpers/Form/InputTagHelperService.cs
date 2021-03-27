﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Faraboom.Framework.Core;
using Faraboom.Framework.Core.Extensions;
using Faraboom.Framework.DataAnnotation;
using Faraboom.Framework.UI.Bootstrap.TagHelpers.Extensions;

using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Form
{
    [Injectable]
    public class InputTagHelperService : TagHelperService<InputTagHelper>
    {
        private readonly IHtmlGenerator generator;
        private readonly HtmlEncoder encoder;
        private IEnumerable<Attribute> cachedModelAttributes;

        public InputTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder)
        {
            this.generator = generator;
            this.encoder = encoder;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            cachedModelAttributes = TagHelper.For.ModelExplorer.GetAttributes();

            var (innerHtml, isCheckBox) = await GetFormInputGroupAsHtmlAsync(context, output);

            var order = TagHelper.For.ModelExplorer.GetDisplayOrder();

            AddGroupToFormGroupContents(
                context,
                TagHelper.For.Name,
                SurroundInnerHtmlAndGet(context, output, innerHtml, isCheckBox),
                order,
                out var suppress
            );

            if (suppress)
            {
                output.SuppressOutput();
            }
            else
            {
                output.TagMode = TagMode.StartTagAndEndTag;
                output.TagName = "div";
                LeaveOnlyGroupAttributes(context, output);
                output.Attributes.AddClass(isCheckBox ? "custom-checkbox" : "form-group");
                output.Attributes.AddClass(isCheckBox ? "custom-control" : "");
                output.Attributes.AddClass(isCheckBox ? "mb-2" : "");
                output.Content.SetHtmlContent(output.Content.GetContent() + innerHtml);
            }
        }

        protected virtual async Task<(string, bool)> GetFormInputGroupAsHtmlAsync(TagHelperContext context, TagHelperOutput output)
        {
            var (inputTag, isCheckBox) = await GetInputTagHelperOutputAsync(context, output);

            var inputHtml = inputTag.Render(encoder);
            var label = await GetLabelAsHtmlAsync(context, output, inputTag, isCheckBox);
            var info = GetInfoAsHtml(context, output, inputTag, isCheckBox);
            var validation = isCheckBox ? "" : await GetValidationAsHtmlAsync(context, output, inputTag);

            return (GetContent(context, output, label, inputHtml, validation, info, isCheckBox), isCheckBox);
        }

        protected virtual async Task<string> GetValidationAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag)
        {
            if (IsOutputHidden(inputTag))
                return "";

            var validationMessageTagHelper = new ValidationMessageTagHelper(generator)
            {
                For = TagHelper.For,
                ViewContext = TagHelper.ViewContext
            };

            var attributeList = new TagHelperAttributeList { { "class", "text-danger" } };

            return await validationMessageTagHelper.RenderAsync(attributeList, context, encoder, "span", TagMode.StartTagAndEndTag);
        }

        protected virtual string GetContent(TagHelperContext context, TagHelperOutput output, string label, string inputHtml, string validation, string infoHtml, bool isCheckbox)
        {
            var innerContent = isCheckbox ?
                inputHtml + label :
                label + inputHtml;

            return innerContent + infoHtml + validation;
        }

        protected virtual string SurroundInnerHtmlAndGet(TagHelperContext context, TagHelperOutput output, string innerHtml, bool isCheckbox)
        {
            return "<div class=\"" + (isCheckbox ? "custom-checkbox custom-control" : "form-group") + "\">" +
                   Environment.NewLine + innerHtml + Environment.NewLine +
                   "</div>";
        }

        protected virtual Microsoft.AspNetCore.Razor.TagHelpers.TagHelper GetInputTagHelper(TagHelperContext context, TagHelperOutput output)
        {
            var dataTypeAttribute = cachedModelAttributes?.GetAttribute<DataTypeAttribute>();
            if (dataTypeAttribute?.ElementDataType == ElementDataType.MultilineText)
            {
                var textAreaTagHelper = new TextAreaTagHelper(generator)
                {
                    For = TagHelper.For,
                    ViewContext = TagHelper.ViewContext
                };

                if (!string.IsNullOrWhiteSpace(TagHelper.Name))
                    textAreaTagHelper.Name = TagHelper.Name;

                return textAreaTagHelper;
            }

            var inputTagHelper = new Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper(generator)
            {
                For = TagHelper.For,
                InputTypeName = TagHelper.InputTypeName,
                ViewContext = TagHelper.ViewContext
            };

            if (!TagHelper.Format.IsNullOrEmpty())
                inputTagHelper.Format = TagHelper.Format;

            if (!TagHelper.Name.IsNullOrEmpty())
                inputTagHelper.Name = TagHelper.Name;

            if (!(TagHelper.Value as string).IsNullOrEmpty())
                inputTagHelper.Value = (TagHelper.Value as string);

            return inputTagHelper;
        }

        protected virtual async Task<(TagHelperOutput, bool)> GetInputTagHelperOutputAsync(TagHelperContext context, TagHelperOutput output)
        {
            var tagHelper = GetInputTagHelper(context, output);

            var inputTagHelperOutput = await tagHelper.ProcessAndGetOutputAsync(
                GetInputAttributes(context, output),
                context,
                "input"
            );

            ConvertToTextAreaIfTextArea(inputTagHelperOutput);
            AddDisabledAttribute(inputTagHelperOutput);
            AddAutoFocusAttribute(inputTagHelperOutput);
            var isCheckbox = IsInputCheckbox(context, output, inputTagHelperOutput.Attributes);
            AddFormControlClass(context, output, isCheckbox, inputTagHelperOutput);
            AddReadOnlyAttribute(inputTagHelperOutput);
            AddPlaceholderAttribute(inputTagHelperOutput);
            AddInfoTextId(inputTagHelperOutput);

            return (inputTagHelperOutput, isCheckbox);
        }

        private void AddFormControlClass(TagHelperContext context, TagHelperOutput output, bool isCheckbox, TagHelperOutput inputTagHelperOutput)
        {
            var className = "form-control";

            if (isCheckbox)
            {
                className = "custom-control-input";
            }

            inputTagHelperOutput.Attributes.AddClass(className + " " + GetSize(context, output));
        }

        protected virtual void AddAutoFocusAttribute(TagHelperOutput inputTagHelperOutput)
        {
            if (TagHelper.AutoFocus && !inputTagHelperOutput.Attributes.ContainsName("data-auto-focus"))
            {
                inputTagHelperOutput.Attributes.Add("data-auto-focus", "true");
            }
        }

        protected virtual void AddDisabledAttribute(TagHelperOutput inputTagHelperOutput)
        {
            if (!inputTagHelperOutput.Attributes.ContainsName("disabled") &&
                     (TagHelper.IsDisabled || cachedModelAttributes?.GetAttribute<UIHintAttribute>()?.Disabled == true))
            {
                inputTagHelperOutput.Attributes.Add("disabled", "");
            }
        }

        protected virtual void AddReadOnlyAttribute(TagHelperOutput inputTagHelperOutput)
        {
            if (!inputTagHelperOutput.Attributes.ContainsName("readonly") &&
                    (TagHelper.IsReadonly.GetValueOrDefault() || cachedModelAttributes?.GetAttribute<UIHintAttribute>()?.Readonly == true))
            {
                inputTagHelperOutput.Attributes.Add("readonly", "");
            }
        }

        protected virtual void AddPlaceholderAttribute(TagHelperOutput inputTagHelperOutput)
        {
            if (inputTagHelperOutput.Attributes.ContainsName("placeholder"))
                return;

            var attribute = cachedModelAttributes?.GetAttribute<DisplayAttribute>();
            if (attribute != null)
            {
                var placeholderLocalized = Globals.GetLocalizedValueInternal(attribute, TagHelper.For.Name, Constants.ResourceKey.Prompt);
                if (!string.IsNullOrWhiteSpace(placeholderLocalized))
                    inputTagHelperOutput.Attributes.Add("placeholder", placeholderLocalized);
            }
        }

        protected virtual void AddInfoTextId(TagHelperOutput inputTagHelperOutput)
        {
            var idAttr = inputTagHelperOutput.Attributes.FirstOrDefault(a => a.Name == "id");
            if (idAttr == null)
                return;

            var attribute = cachedModelAttributes?.GetAttribute<DisplayAttribute>();
            if (attribute != null)
            {
                var description = Globals.GetLocalizedValueInternal(attribute, TagHelper.For.Name, Constants.ResourceKey.Description);
                if (!string.IsNullOrWhiteSpace(description))
                    inputTagHelperOutput.Attributes.Add("aria-describedby", description);
            }
        }

        protected virtual bool IsInputCheckbox(TagHelperContext context, TagHelperOutput output, TagHelperAttributeList attributes)
        {
            return attributes.Any(t => t.Value != null && t.Name == "type" && t.Value.ToString() == "checkbox");
        }

        protected virtual async Task<string> GetLabelAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag, bool isCheckbox)
        {
            if (IsOutputHidden(inputTag) || TagHelper.SuppressLabel)
                return "";

            var uIHintAttribute = cachedModelAttributes?.GetAttribute<UIHintAttribute>();
            if (uIHintAttribute != null && uIHintAttribute.LabelPosition == LabelPosition.Hidden)
                return "";

            if (string.IsNullOrEmpty(TagHelper.Label))
                return await GetLabelAsHtmlUsingTagHelperAsync(context, output, isCheckbox) + GetRequiredSymbol(context, output);

            var checkboxClass = isCheckbox ? "class=\"custom-control-label\" " : "";

            return "<label " + checkboxClass + GetIdAttributeAsString(inputTag) + ">"
                   + TagHelper.Label +
                   "</label>" + GetRequiredSymbol(context, output);
        }

        protected virtual string GetRequiredSymbol(TagHelperContext context, TagHelperOutput output)
        {
            if (!TagHelper.DisplayRequiredSymbol)
                return "";

            return cachedModelAttributes?.GetAttribute<RequiredAttribute>() != null ? "<span> * </span>" : "";
        }

        protected virtual string GetInfoAsHtml(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag, bool isCheckbox)
        {
            if (IsOutputHidden(inputTag))
                return "";

            if (isCheckbox)
                return "";

            var text = "";
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
                        text = description;
                }
            }

            if (string.IsNullOrEmpty(text))
                return "";

            var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");
            return $"<small id=\"{idAttr?.Value}InfoText\" class=\"form-text text-muted\">{text}</small>";
        }

        protected virtual async Task<string> GetLabelAsHtmlUsingTagHelperAsync(TagHelperContext context, TagHelperOutput output, bool isCheckbox)
        {
            var labelTagHelper = new LabelTagHelper(generator)
            {
                For = TagHelper.For,
                ViewContext = TagHelper.ViewContext
            };

            var attributeList = new TagHelperAttributeList();

            if (isCheckbox)
                attributeList.AddClass("custom-control-label");

            return await labelTagHelper.RenderAsync(attributeList, context, encoder, "label", TagMode.StartTagAndEndTag);
        }

        protected virtual void ConvertToTextAreaIfTextArea(TagHelperOutput tagHelperOutput)
        {
            var dataTypeAttribute = cachedModelAttributes?.GetAttribute<DataTypeAttribute>();
            if (dataTypeAttribute == null || dataTypeAttribute.ElementDataType != ElementDataType.MultilineText)
                return;

            var uIHintAttribute = cachedModelAttributes?.GetAttribute<UIHintAttribute>();
            tagHelperOutput.TagName = "textarea";
            tagHelperOutput.TagMode = TagMode.StartTagAndEndTag;
            tagHelperOutput.Content.SetContent(TagHelper.For.ModelExplorer.Model?.ToString());
            if (uIHintAttribute?.Rows > 0)
                tagHelperOutput.Attributes.Add("rows", uIHintAttribute.Rows);

            if (uIHintAttribute?.Cols > 0)
                tagHelperOutput.Attributes.Add("cols", uIHintAttribute.Cols);
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

            if (!TagHelper.InputTypeName.IsNullOrEmpty() && !attrList.ContainsName("type"))
            {
                attrList.Add("type", TagHelper.InputTypeName);
            }

            if (!TagHelper.Name.IsNullOrEmpty() && !attrList.ContainsName("name"))
            {
                attrList.Add("name", TagHelper.Name);
            }

            if (!(TagHelper.Value as string).IsNullOrEmpty() && !attrList.ContainsName("value"))
            {
                attrList.Add("value", TagHelper.Value);
            }

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

        protected virtual string GetSize(TagHelperContext context, TagHelperOutput output)
        {
            var uIHintAttribute = cachedModelAttributes?.GetAttribute<UIHintAttribute>();
            if (uIHintAttribute != null)
                TagHelper.Size = uIHintAttribute.Size;

            switch (TagHelper.Size)
            {
                case FormControlSize.Small:
                    return "custom-select-sm";
                case FormControlSize.Medium:
                    return "custom-select-md";
                case FormControlSize.Large:
                    return "custom-select-lg";
                default:
                    return "";
            }
        }

        protected virtual bool IsOutputHidden(TagHelperOutput inputTag)
        {
            var dataTypeAttribute = cachedModelAttributes?.GetAttribute<DataTypeAttribute>();
            return inputTag.Attributes.Any(t => t.Name.ToLowerInvariant() == "type" && t.Value.ToString().ToLowerInvariant() == "hidden")
                || dataTypeAttribute?.ElementDataType == ElementDataType.Hidden;
        }

        protected virtual string GetIdAttributeAsString(TagHelperOutput inputTag)
        {
            var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");

            return idAttr != null ? "for=\"" + idAttr.Value + "\"" : "";
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
                    PropertyName = propertyName
                });
            }
        }
    }
}