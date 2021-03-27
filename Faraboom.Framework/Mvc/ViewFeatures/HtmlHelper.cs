﻿using Faraboom.Framework.Core;

using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;
using System.Reflection;

namespace Faraboom.Framework.Mvc.ViewFeatures
{
    public class HtmlHelper
    {
        public static IEnumerable<SelectListItem> GetExtendedEnumSelectList<TEnum>() where TEnum : struct
        {
            var fields = typeof(TEnum).GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField);

            var selectList = new List<SelectListItem>();
            var groupList = new Dictionary<string, SelectListGroup>();

            foreach (var field in fields)
            {
                var selectListItem = new SelectListItem
                {
                    Text = Globals.GetLocalizedDisplayName(field),
                    Value = field.GetValue(field).ToString(),
                };

                //if (!string.IsNullOrEmpty(keyValuePair.Key.Group))
                //{
                //    if (!groupList.ContainsKey(keyValuePair.Key.Group))
                //    {
                //        groupList[keyValuePair.Key.Group] = new SelectListGroup() { Name = keyValuePair.Key.Group };
                //    }

                //    selectListItem.Group = groupList[keyValuePair.Key.Group];
                //}

                selectList.Add(selectListItem);
            }

            return selectList;
        }
    }
}
