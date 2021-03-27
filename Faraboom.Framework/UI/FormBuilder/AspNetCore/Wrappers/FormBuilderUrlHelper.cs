using Microsoft.AspNetCore.Mvc;

namespace Faraboom.Framework.UI.FormBuilder.AspNetCore.Wrappers
{
    class FormBuilderUrlHelper : UrlHelper
    {
        private readonly IUrlHelper mvcUrlHelper;

        public FormBuilderUrlHelper(IUrlHelper mvcUrlHelper)
        {
            this.mvcUrlHelper = mvcUrlHelper;
        }

        //public string Action<TController>(Expression<Action<TController>> action)
        //    where TController : Controller
        //{
        //    var routeValuesFromExpression = Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression<TController>(action);
        //    var actionName = routeValuesFromExpression["action"].ToString();
        //    return mvcUrlHelper.Action(actionName);
        //}

        public string Action(string actionName, string controllerName)
        {
            return mvcUrlHelper.Action(actionName, controllerName);
        }
 
    }
}