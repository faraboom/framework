using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Faraboom.Framework.UI.FormBuilder
{
    public interface IFormBuilderHtmlHelper
    {
        UrlHelper Url();
        string WriteTypeToString(Type type);
        ViewData ViewData { get; }
        IViewFinder ViewFinder { get; }
        //string Partial(string partialName, object vm); 
        void RenderPartial(string partialName, object model);
        PropertyViewModel CreatePropertyViewModel(Type objectType, string name);

    }

    public interface IFormBuilderHtmlHelper<TViewData> : IFormBuilderHtmlHelper
    {
        //string Partial(string partialName, object vm, TViewData viewData);
    }
    public interface IViewFinder
    {
        IViewFinderResult FindPartialView(string partialViewName);
    }

    public interface IViewFinderResult
    {
        View View { get; }
    }
    public class View
    {
        
    }


    public class ViewData 
    {
        public ViewData(IModelStateDictionary modelState, object model)
        {
            ModelState = modelState;
            Model = model;
        }

        public ViewData()
        {
            ModelState = new FormBuilderModelStateDictionary();
        }

        public IModelStateDictionary ModelState { get; private set; }
        public object Model { get; private set; }
    }


    public class FormBuilderModelStateDictionary : Dictionary<string, ModelState>, IModelStateDictionary
    {
        public bool IsValid => Values.SelectMany(v => v.Errors).Any(e => e != null) == false;
    }

    public interface IModelStateDictionary
    {
        bool TryGetValue(string key, out ModelState modelState);
        ModelState this[string key] { get; }
        bool ContainsKey(string key);
        bool IsValid { get; }
    }

    public class ModelState
    {
        public ModelState()
        {
                
        }

        public ModelState(FormBuilderModelStateErrors errors, FormBuilderModelStateValue value)
        {
            Errors = errors;
            Value = value;
        }

        public FormBuilderModelStateValue Value { get; private set; }
        public FormBuilderModelStateErrors Errors { get; private set; }
    }

    public class FormBuilderModelStateErrors : IEnumerable<FormBuilderModelStateError>
    {
        private IEnumerable<FormBuilderModelStateError> _items;

        public FormBuilderModelStateErrors(IEnumerable<FormBuilderModelStateError> items)
        {
            _items = items;
        }

        public IEnumerator<FormBuilderModelStateError> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class FormBuilderModelStateError
    {
        public string ErrorMessage { get; set; }
    }

    public class FormBuilderModelStateValue
    {
        public FormBuilderModelStateValue(object attemptedValue)
        {
            AttemptedValue = attemptedValue;
        }

        public object AttemptedValue { get; private set; }
    }

    public interface UrlHelper
    {
        string Action(string actionName, string controllerName);
    }
}