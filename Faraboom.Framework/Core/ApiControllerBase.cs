using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Faraboom.Framework.Core
{
    [ApiController]
    //[Route("api/[[area]]/[[controller]]/[[action]]")]
    [Route("{culture=fa}/api/{area:slugify:exists}/{controller:slugify=Home}/{action:slugify=Index}/{id?}")]
    public abstract class ApiControllerBase<TClass, TUser> : ControllerBase
        where TClass : class
        where TUser : class
    {
        protected readonly ILogger Logger;
        protected readonly UserManager<TUser> UserManager;
        protected readonly IStringLocalizer<TClass> Localizer;

        public ApiControllerBase(UserManager<TUser> userManager, ILogger<TClass> logger, IStringLocalizer<TClass> localizer)
        {
            Logger = logger;
            UserManager = userManager;
            Localizer = localizer;
        }

        public RedirectToPageResult RedirectToAreaPage(string pageName, string area, object routeValues = null)
            => base.RedirectToPage(pageName.TrimEnd(Constants.PagePostfix), Globals.PrepareValues(routeValues, area));

        public RedirectToActionResult RedirectToAreaAction(string actionName, string controllerName, string area, object routeValues = null)
            => base.RedirectToAction(actionName, controllerName.TrimEnd(Constants.ControllerPostfix), Globals.PrepareValues(routeValues, area));

        public override RedirectToActionResult RedirectToAction(string actionName, string controllerName, object routeValues, string fragment)
            => base.RedirectToAction(actionName, controllerName.TrimEnd(Constants.ControllerPostfix), Globals.PrepareValues(routeValues), fragment);

        public override RedirectToPageResult RedirectToPage(string pageName, string pageHandler, object routeValues, string fragment)
            => base.RedirectToPage(pageName.TrimEnd(Constants.PagePostfix), pageHandler, Globals.PrepareValues(routeValues), fragment);
    }
}
