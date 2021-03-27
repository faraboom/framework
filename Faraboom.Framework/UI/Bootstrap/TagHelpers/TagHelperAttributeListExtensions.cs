﻿using Faraboom.Framework.Core.Extensions.Collections.Generic;

using Microsoft.AspNetCore.Razor.TagHelpers;

using System;
using System.Linq;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers
{
    public static class TagHelperAttributeListExtensions
    {
        public static void AddClass(this TagHelperAttributeList attributes, string className)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                return;
            }

            var classAttribute = attributes["class"];
            if (classAttribute == null)
            {
                attributes.Add("class", className);
            }
            else
            {
                var existingClasses = classAttribute.Value.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                existingClasses.AddIfNotContains(className);
                attributes.SetAttribute("class", string.Join(" ", existingClasses));
            }
        }

        public static string GetClass(this TagHelperAttributeList attributes)
        {
            var classAttribute = attributes["class"];
            return classAttribute == null ? "" : classAttribute.Value.ToString();
        }

        public static void RemoveClass(this TagHelperAttributeList attributes, string className)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                return;
            }

            var classAttribute = attributes["class"];
            if (classAttribute == null)
            {
                return;
            }

            var classList = classAttribute.Value.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            classList.RemoveAll(c => c == className);

            attributes.SetAttribute("class", string.Join(" ", classList));
        }

        public static void AddIfNotExist(this TagHelperAttributeList attributes, string name, object value)
        {
            if (!attributes.ContainsName(name))
                attributes.Add(name, value);
        }

        public static void Merge(this TagHelperAttributeList attributes, TagHelperAttributeList helperAttributes)
        {
            foreach (var item in helperAttributes)
            {
                attributes.AddIfNotExist(item.Name, item.Value);
            }
        }
    }
}
