using System;
using System.Linq;

using Faraboom.Framework.UI.FormBuilder.ViewHelpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace Faraboom.Framework.UI.FormBuilder.AspNetCore.Wrappers
{
    public class FormBuilderHtmlHelper : IFormBuilderHtmlHelper<ViewDataDictionary>
    {
        public FormBuilderHtmlHelper(IHtmlHelper helper)
        {
            this.InnerHtmlHelper = helper;
        }

        public UrlHelper Url()
        {
            var urlHelper = InnerHtmlHelper.ViewContext.HttpContext.RequestServices.GetService<IUrlHelper>();
            return new FormBuilderUrlHelper(urlHelper);
        }

        public string WriteTypeToString(Type type)
        {
            throw new NotImplementedException(nameof(type));
            //return new Encoder().WriteTypeToString(type);
        }


        public ViewData ViewData => new FormBuilderViewData(InnerHtmlHelper.ViewData);

        public IViewFinder ViewFinder => new FormBuilderContext(InnerHtmlHelper.ViewContext);

        public IHtmlHelper InnerHtmlHelper { get; }

        public string Partial(string partialName, object vm, ViewDataDictionary viewData)
        {
            return InnerHtmlHelper.Partial(partialName, vm, viewData).ToHtmlString();
        }

        public string Partial(string partialName, object vm)
        {
            return Partial(partialName, vm, null);
        }

        public string Raw(string s)
        {
            return InnerHtmlHelper.Raw(s).ToHtmlString();
        }

        public void RenderPartial(string partialName, object model)
        {
            this.InnerHtmlHelper.RenderPartialAsync(partialName, model, null).RunSynchronously();
        }

        public PropertyViewModel CreatePropertyViewModel(Type objectType, string name)
        {
            return new PropertyViewModel(objectType, name);
        }

        public ObjectChoices[] Choices(PropertyViewModel model) //why is this needed? HM
        {
            return (from obj in model.Choices.Cast<object>().ToArray()
                   let choiceType = obj == null ? model.Type : obj.GetType()
                   let properties = FB.PropertiesFor(obj, choiceType)
                       .Each(p => p.Name = model.Name + "." + p.Name)
                       .Each(p => p.Readonly |= model.Readonly)
                       .Each(p => p.Id = Guid.NewGuid().ToString())
                   select new ObjectChoices
                   {
                       obj = obj,
                       choiceType = choiceType,
                       properties = properties,
                       name = obj != null ? obj.DisplayName() : choiceType.DisplayName()
                   }).ToArray();
        }
    }
}