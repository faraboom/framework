using System.Collections.Generic;
using System.Text;

using Faraboom.Framework.UI.FormBuilder.AspNetCore.Wrappers;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Faraboom.Framework.UI.FormBuilder.AspNetCore
{
    public static class RenderExtension
    {
        public static IHtmlContent Render(this IEnumerable<PropertyViewModel> propertyViewModels, IHtmlHelper html)
        {
            var sb = new StringBuilder();
            foreach (var propertyViewModel in propertyViewModels)
            {
                sb.AppendLine(propertyViewModel.Render(html).ToHtmlString());
            }
            return new HtmlString(sb.ToString());
        }

        public static IHtmlContent Render(this PropertyViewModel propertyViewModel, IHtmlHelper html)
        {
            return html.Partial("FormBuilder/Form.Property", propertyViewModel);
        }

        public static IHtmlContent Render(this FormViewModel formViewModel, IHtmlHelper html)
        {
            return html.Partial("FormBuilder/Form", formViewModel);
        }
    }
}