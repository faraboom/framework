using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

using Faraboom.Framework.UI.FormBuilder.AspNetCore.Wrappers;
using Faraboom.Framework.UI.FormBuilder.ViewHelpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Faraboom.Framework.UI.FormBuilder.AspNetCore
{
    public static class FormHelperExtension
    {

        public static FormViewModel FormForAction(this IHtmlHelper<dynamic> html, Expression<Func<ControllerBase, Task<IActionResult>>> action)
        {
            return FormFor(html, action.MethodInfo());
        }

        public static FormViewModel FormForAction<TArg1>(this IHtmlHelper<dynamic> html, Expression<Func<ControllerBase, TArg1, Task<IActionResult>>> action)
        {
            return FormFor(html, action.MethodInfo());
        }

        public static FormViewModel FormForAction<TController, TArg1, TArg2, TActionResult>(
            this IHtmlHelper<dynamic> html, Expression<Func<TController, TArg1, TArg2, TActionResult>> action)
            where TController : ControllerBase
            where TActionResult : IActionResult
        {
            return FormFor(html, action.MethodInfo());
        }

        public static FormViewModel FormForAction0<TArg1, TArg2>(this IHtmlHelper<dynamic> html, Expression<Func<ControllerBase, TArg1, TArg2, IActionResult>> action)
        {
            return FormFor(html, action.MethodInfo());
        }

        public static FormViewModel FormForAction<TArg1, TArg2, TArg3>(this IHtmlHelper<dynamic> html, Expression<Func<ControllerBase, TArg1, TArg2, TArg3, Task<IActionResult>>> action)
        {
            return FormFor(html, action.MethodInfo());
        }

        public static FormViewModel FormForAction<TArg1, TArg2, TArg3, TArg4>(this IHtmlHelper<dynamic> html, Expression<Func<ControllerBase, TArg1, TArg2, TArg3, TArg4, Task<IActionResult>>> action)
        {
            return FormFor(html, action.MethodInfo());
        }

        public static FormViewModel FormForAction<TArg1, TArg2, TArg3, TArg4, TArg5>(this IHtmlHelper<dynamic> html, Expression<Func<ControllerBase, TArg1, TArg2, TArg3, TArg4, TArg5, Task<IActionResult>>> action)
        {
            return FormFor(html, action.MethodInfo());
        }

        public static FormViewModel FormForAction<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(this IHtmlHelper<dynamic> html, Expression<Func<ControllerBase, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, Task<IActionResult>>> action)
        {
            return FormFor(html, action.MethodInfo());
        }

        public static PropertyViewModel CreatePropertyViewModel(this IHtmlHelper helper, Type objectType, string name)
        {
            return new Wrappers.FormBuilderHtmlHelper(helper).CreatePropertyViewModel(objectType, name);
        }

        public static ObjectChoices[] Choices(this IHtmlHelper html, PropertyViewModel model)
        {
            return new Wrappers.FormBuilderHtmlHelper(html).Choices(model);
        }


        public static MethodInfo MethodInfo(this Expression method)
        {
            var lambda = method as LambdaExpression;
            if (lambda == null)
                throw new ArgumentNullException("method");
            MethodCallExpression methodExpr = null;
            if (lambda.Body.NodeType == ExpressionType.Call)
                methodExpr = lambda.Body as MethodCallExpression;

            if (methodExpr == null)
                throw new ArgumentNullException("method");
            return methodExpr.Method;
        }

        private static FormViewModel FormFor(this IHtmlHelper<dynamic> html, MethodInfo mi)
        {
            var formBuilderHtmlHelper = new FormBuilderHtmlHelper(html);
            var actionName = mi.Name;
            var controllerTypeName = mi.ReflectedType.Name;
            var controllerName = controllerTypeName.Substring(0, controllerTypeName.LastIndexOf("Controller"));


            return new FormViewModel(mi, formBuilderHtmlHelper.Url().Action(actionName, controllerName));
        }

        private static bool IsNullable<T>(T t)
        {
            return false;
        }

        private static bool IsNullable<T>(T? t) where T : struct
        {
            return true;
        }
    }
}