namespace Faraboom.Framework.Startup
{
    using System.Linq;
    using Faraboom.Framework.Core;
    using Faraboom.Framework.Localization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.DependencyInjection;

    public static class LocalizationExtensions
    {
        public static void ConfigureRequestLocalization(this IServiceCollection services)
        {
            var supportedCultures = CultureExtensions.GetAtomicValues().Select(t => Globals.GetCulture(t)).ToArray();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(Constants.DefaultLanguageCode);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders.Insert(0, new RouteValueRequestCultureProvider(supportedCultures));
            });
        }
    }
}
