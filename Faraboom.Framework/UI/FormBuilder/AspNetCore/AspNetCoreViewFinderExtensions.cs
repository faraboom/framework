using System;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Faraboom.Framework.UI.FormBuilder.AspNetCore
{
    public static class AspNetCoreViewFinderExtensions
    {
        public static IHtmlContent BestProperty(this IHtmlHelper html, PropertyViewModel vm)
        {
            string viewName = ViewFinderExtensions.BestPropertyName(new Wrappers.FormBuilderHtmlHelper(html), vm);
            return html.Partial(viewName, vm);
        }

        public static string BestViewName(this IHtmlHelper html, Type type, string prefix = null)
        {
            return ViewFinderExtensions.BestViewName(new Wrappers.FormBuilderHtmlHelper(html), type, prefix);
        }

        public static IHtmlContent BestPartial(this IHtmlHelper html, object model, Type type = null, string prefix = null)
        {
            var partialViewName = BestViewName(html, type ?? model.GetType(), prefix);
            return html.Partial(partialViewName, model);
        }

        public static IHtmlContent Partial(this IHtmlHelper html, string partialViewName, object model, ViewDataDictionary vdd = null)
        {
            return html.PartialAsync(partialViewName, model, vdd).Result;
        }
    }
}