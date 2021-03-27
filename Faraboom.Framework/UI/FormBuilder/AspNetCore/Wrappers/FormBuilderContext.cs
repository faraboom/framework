using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.DependencyInjection;

namespace Faraboom.Framework.UI.FormBuilder.AspNetCore.Wrappers
{
    public class FormBuilderContext : IViewFinder
    {
        private readonly ViewContext viewContext;

        public FormBuilderContext(ViewContext viewContext)
        {
            this.viewContext = viewContext;
        }

        public IViewFinderResult FindPartialView(string partialViewName)
        {
            var ac = new ActionContext(viewContext.HttpContext, viewContext.RouteData, viewContext.ActionDescriptor);
            var service = viewContext.HttpContext.RequestServices.GetService<ICompositeViewEngine>();
            var viewEngineResult = service.FindView(ac, partialViewName, false);

            return viewEngineResult == null ? null : new FormBuilderViewFinderResult(viewEngineResult);
        }
    }
}