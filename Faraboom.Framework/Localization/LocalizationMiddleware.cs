using Faraboom.Framework.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Faraboom.Framework.Localization
{
    internal class LocalizationMiddleware
    {
        private readonly RequestDelegate next;

        public LocalizationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var culture = httpContext.GetRouteData()?.Values[Constants.LanguageIdentifier] as string;
            if (!string.IsNullOrWhiteSpace(culture) && CultureExtensions.GetAtomicValues().Contains(culture))
            {
                CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = Globals.GetCulture(culture);
            }
            //if(httpContext.Request.Cookies.ContainsKey(Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.DefaultCookieName))

            return next(httpContext);
        }
    }
}
