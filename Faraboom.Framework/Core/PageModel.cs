namespace Faraboom.Framework.Core
{
    using System.Linq;

    using Faraboom.Framework.Core.Utils.Export;
    using Faraboom.Framework.Data;
    using Faraboom.Framework.Service.Factory;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static Faraboom.Framework.Core.Constants;

    public abstract class PageModel<TClass, TUser> : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
        where TClass : class
        where TUser : class
    {
        private readonly IGenericFactory<ExportBase, ExportType> genericFactory;
        private PagingDto pagingDto;
        private ISearch search;

        protected PageModel(UserManager<TUser> userManager, ILogger<TClass> logger, IStringLocalizer<TClass> localizer,
            IGenericFactory<ExportBase, ExportType> genericFactory)
        {
            Logger = logger;
            UserManager = userManager;
            Localizer = localizer;
            this.genericFactory = genericFactory;
        }

        protected ILogger Logger { get; }

        protected UserManager<TUser> UserManager { get; }

        protected IStringLocalizer<TClass> Localizer { get; }

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
            else
            {
                pagingDto = context.HandlerArguments.Any(t => t.Value?.GetType() == typeof(PagingDto)) ? context.HandlerArguments.First(t => t.Value?.GetType() == typeof(PagingDto)).Value as PagingDto : null;
                search = context.HandlerArguments.Any(t => t.Value?.GetType() == typeof(ISearch)) ? context.HandlerArguments.First(t => t.Value?.GetType() == typeof(ISearch)).Value as ISearch : null;
            }
        }

        public override void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            base.OnPageHandlerExecuted(context);

            if (pagingDto?.Export is true)
            {
                if ((context.Result as ObjectResult)?.Value is GridDataSource gridDataSource)
                {
                    var fileName = context.ActionDescriptor.DisplayName.Split('/', System.StringSplitOptions.RemoveEmptyEntries).Last();
                    context.Result = genericFactory.GetProvider(pagingDto.ExportType, false).Export(gridDataSource, search, fileName);
                }
            }
        }

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
