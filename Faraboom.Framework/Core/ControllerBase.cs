namespace Faraboom.Framework.Core
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    public abstract class ControllerBase<TClass, TUser> : Controller
        where TClass : class
        where TUser : class
    {
        protected ControllerBase(UserManager<TUser> userManager, ILogger<TClass> logger, IStringLocalizer<TClass> localizer)
        {
            Logger = logger;
            UserManager = userManager;
            Localizer = localizer;
        }

        protected ILogger Logger { get; }

        protected UserManager<TUser> UserManager { get; }

        protected IStringLocalizer<TClass> Localizer { get; }

        public RedirectToPageResult RedirectToAreaPage(string pageName, string area, object routeValues = null)
            => RedirectToPage(pageName.TrimEnd(Constants.PagePostfix), Globals.PrepareValues(routeValues, area));

        public RedirectToActionResult RedirectToAreaAction(string actionName, string controllerName, string area, object routeValues = null)
            => RedirectToAction(actionName, controllerName.TrimEnd(Constants.ControllerPostfix), Globals.PrepareValues(routeValues, area));

        public override RedirectToActionResult RedirectToAction(string actionName, string controllerName, object routeValues, string fragment)
            => base.RedirectToAction(actionName, controllerName.TrimEnd(Constants.ControllerPostfix), Globals.PrepareValues(routeValues), fragment);

        public override RedirectToPageResult RedirectToPage(string pageName, string pageHandler, object routeValues, string fragment)
            => base.RedirectToPage(pageName.TrimEnd(Constants.PagePostfix), pageHandler, Globals.PrepareValues(routeValues), fragment);
    }
}
