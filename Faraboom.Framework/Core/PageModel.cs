﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using System.Linq;

namespace Faraboom.Framework.Core
{
    public abstract class PageModel<TClass, TUser> : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
        where TClass : class
        where TUser : class
    {
        protected readonly ILogger Logger;
        protected readonly UserManager<TUser> UserManager;
        protected readonly IStringLocalizer<TClass> Localizer;

        protected PageModel(UserManager<TUser> userManager, ILogger<TClass> logger, IStringLocalizer<TClass> localizer)
        {
            Logger = logger;
            UserManager = userManager;
            Localizer = localizer;
        }

        public override BadRequestObjectResult BadRequest(object error)
        {
            return base.BadRequest(error);
        }

        public override NotFoundObjectResult NotFound(object value)
        {
            return base.NotFound(value);
        }

        public override ForbidResult Forbid()
        {
            return base.Forbid();
        }

        public override ObjectResult StatusCode(int statusCode, object value)
        {
            return base.StatusCode(statusCode, value);
        }

        public override void OnPageHandlerExecuting(Microsoft.AspNetCore.Mvc.Filters.PageHandlerExecutingContext context)
        {
            base.OnPageHandlerExecuting(context);

            if (!context.ModelState.IsValid)
            {
                if (context.HttpContext.Request?.Headers["X-Requested-With"].FirstOrDefault() == "XMLHttpRequest")
                {
                    var errors = ModelState.Values.SelectMany(t => t.Errors.Select(e => e.ErrorMessage));
                    context.Result = new BadRequestObjectResult(errors);
                }
                else
                {
                    context.Result = Page();
                }
            }
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
