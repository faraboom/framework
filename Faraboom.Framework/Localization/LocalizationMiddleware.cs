namespace Faraboom.Framework.Localization
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Faraboom.Framework.Core;
    using Faraboom.Framework.Core.Extensions;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class LocalizationMiddleware
    {
        private readonly RequestDelegate next;

        public LocalizationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var culture = httpContext.GetRouteData()?.Values[Constants.LanguageIdentifier] as string;
            if (!culture.IsNullOrEmpty() && CultureExtensions.GetAtomicValues().Contains(culture))
            {
                CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = Globals.GetCulture(culture);
            }

            // if(httpContext.Request.Cookies.ContainsKey(Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.DefaultCookieName))
            return next(httpContext);
        }
    }
}
