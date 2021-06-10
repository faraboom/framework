namespace Faraboom.Framework.Mvc.Filters
{
    using System.Linq;

    using Faraboom.Framework.Core;
    using Faraboom.Framework.Core.Utils.Export;
    using Faraboom.Framework.Data;
    using Faraboom.Framework.Service.Factory;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ExportableActionFilter : IActionFilter
    {
        private readonly IGenericFactory<ExportBase, Constants.ExportType> genericFactory;
        private PagingDto pagingDto;
        private ISearch search;

        public ExportableActionFilter(IGenericFactory<ExportBase, Constants.ExportType> genericFactory)
        {
            this.genericFactory = genericFactory;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            pagingDto = context.ActionArguments.Any(t => t.Value?.GetType() == typeof(PagingDto)) ? context.ActionArguments.First(t => t.Value?.GetType() == typeof(PagingDto)).Value as PagingDto : null;
            search = context.ActionArguments.Any(t => t.Value?.GetType() == typeof(ISearch)) ? context.ActionArguments.First(t => t.Value?.GetType() == typeof(ISearch)).Value as ISearch : null;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (pagingDto?.Export is true)
            {
                if ((context.Result as ObjectResult)?.Value is GridDataSource gridDataSource)
                {
                    var fileName = (context.ActionDescriptor as ControllerActionDescriptor)?.ActionName;
                    context.Result = genericFactory.GetProvider(pagingDto.ExportType, false).Export(gridDataSource, search, fileName);
                }
            }
        }
    }

    public class ExportableAttribute : TypeFilterAttribute
    {
        public ExportableAttribute()
            : base(typeof(ExportableActionFilter))
        {
        }
    }
}
