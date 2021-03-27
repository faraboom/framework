using Faraboom.Framework.Localization;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

using System.Globalization;
using System.Linq;

namespace Faraboom.Framework.Startup
{
    public static class LocalizationExtensions
    {
        public static void ConfigureRequestLocalization(this IServiceCollection services)
        {
            var supportedCultures = CultureExtensions.GetAtomicValues().Select(t => new CultureInfo(t)).ToArray();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(Core.Constants.DefaultLanguageCode);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders.Insert(0, new RouteValueRequestCultureProvider(supportedCultures));
            });
        }
    }
}
