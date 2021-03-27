using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Form
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SelectItemsAttribute : Attribute
    {
        public string ItemsListPropertyName { get; set; }

        public SelectItemsAttribute(string itemsListPropertyName)
        {
            ItemsListPropertyName = itemsListPropertyName;
        }

        public IEnumerable<SelectListItem> GetItems(ModelExplorer explorer)
        {
            var properties = explorer.Container.Properties.Where(p => p.Metadata.PropertyName.Equals(ItemsListPropertyName)).ToList();

            while (!properties.Any())
            {
                explorer = explorer.Container;
                if (explorer.Container == null)
                {
                    return null;
                }

                properties = explorer.Container.Properties.Where(p => p.Metadata.PropertyName.Equals(ItemsListPropertyName)).ToList();
            }

            var selectItems = (properties.First().Model as IEnumerable<SelectListItem>).ToList();

            return selectItems;
        }
    }
}
