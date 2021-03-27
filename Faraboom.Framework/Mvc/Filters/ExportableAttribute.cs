using Faraboom.Framework.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

using System.Linq;

namespace Faraboom.Framework.Mvc.Filters
{
    public class ExportableActionFilter : IActionFilter
    {
        private PagingDto pagingDto;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            pagingDto = context.ActionArguments.Any(t => t.Value?.GetType() == typeof(PagingDto)) ? context.ActionArguments.FirstOrDefault(t => t.Value?.GetType() == typeof(PagingDto)).Value as PagingDto : null;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (pagingDto?.Export == true)
            {
                var data = (context.Result as ObjectResult)?.Value as GridDataSource;
                if (data != null)
                {
                    var file = Core.Utils.Export.CreateExcelDocumentInHttpResponseMessage(data.Data, data.DataTable);
                    context.Result = file.content;
                    context.HttpContext.Response.Headers.Add("content-type", new string[] { "application/ms-excel" });
                    context.HttpContext.Response.Headers.Add("content-length", new string[] { file.size.ToString() });
                    context.HttpContext.Response.Headers.Add("content-disposition", new string[] { $"attachment; filename=\"{context.ActionDescriptor.DisplayName}.xls\"" });
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
