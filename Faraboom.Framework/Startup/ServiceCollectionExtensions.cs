namespace Faraboom.Framework.Startup
{
    using System;
    using Faraboom.Framework.Cookie;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static void ConfigureApplicationCookie(this IServiceCollection services, string loginAction, string loginController, string loginArea = null)
        {
            services.AddTransient((IServiceProvider serviceProvider) =>
            {
                return new CookieAuthenticationEvents(serviceProvider.GetService<IUrlHelperFactory>(), loginAction, loginController, loginArea);
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.EventsType = typeof(CookieAuthenticationEvents);
            });
        }

        public static void ConfigureApplicationCookie(this IServiceCollection services, string loginPageName, string loginArea = null)
        {
            services.AddTransient((IServiceProvider serviceProvider) =>
            {
                return new CookieAuthenticationEvents(serviceProvider.GetService<IUrlHelperFactory>(), page: loginPageName, area: loginArea);
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.EventsType = typeof(CookieAuthenticationEvents);
            });
        }
    }
}