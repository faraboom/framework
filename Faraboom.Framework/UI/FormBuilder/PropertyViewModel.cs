using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Faraboom.Framework.Core;

namespace Faraboom.Framework.UI.FormBuilder
{
    public class PropertyViewModel : IHasDisplayName
    {
        static PropertyViewModel()
        {
        }

        public PropertyViewModel()
        {

        }

        public PropertyViewModel(ParameterInfo pi)
            : this(pi.ParameterType, pi.Name)
        {
            Readonly = !true;
            IsHidden = pi.GetCustomAttributes<HiddenAttribute>(true).Any();
            GetCustomAttributes = () => pi.GetCustomAttributes(true);

            DisplayName = Globals.GetLocalizedDisplayName(pi.Member);
        }
        public PropertyViewModel(ParameterInfo modelParamInfo, PropertyInfo pi)
            : this(pi.PropertyType, pi.Name)
        {
            Readonly = !true;
            IsHidden = pi.GetCustomAttributes<HiddenAttribute>(true).Any();
            GetCustomAttributes = () => pi.GetCustomAttributes(true);

            DisplayName = Globals.GetLocalizedDisplayName(modelParamInfo.Member);
        }

        public IDictionary<string, string> DataAttributes { get; set; }

        public object Source { get; set; }

        public PropertyViewModel(object model, PropertyInfo pi)
            : this(pi.PropertyType, pi.Name)
        {
            Source = model;
            var getMethod = pi.GetMethod;

            if (pi.GetIndexParameters().Any()) getMethod = null; //dont want to get indexed properties

            //if (html.ViewData.ModelState.TryGetValue(pi.Name, out modelState))
            //{
            //    if (modelState.Value != null)
            //        Value = modelState.Value.AttemptedValue;
            //}
            //else 
            if (getMethod != null && model != null)
            {
                Value = getMethod.Invoke(model, null);
            }
            if (model != null)
            {
                MethodInfo choices = model.GetType().GetTypeInfo().GetDeclaredMethod(pi.Name + "_choices");
                if (choices != null)
                {
                    Choices = (IEnumerable)choices.Invoke(model, null);
                }
                MethodInfo suggestions = model.GetType().GetTypeInfo().GetDeclaredMethod(pi.Name + "_suggestions");
                if (suggestions != null)
                {
                    Suggestions = (IEnumerable)suggestions.Invoke(model, null);
                }
                var setter = pi.SetMethod;
                var getter = getMethod;
                Readonly = setter == null;
                Value = getter?.Invoke(model, null);
            }
            GetCustomAttributes = () => pi.GetCustomAttributes(true);
            Readonly = pi.SetMethod == null;
            IsHidden = pi.GetCustomAttributes(true).OfType<HiddenAttribute>().Any();


            DisplayName = Globals.GetLocalizedDisplayName(pi.Member);

            DataAttributes = new Dictionary<string, string>();
        }


        public bool? NotOptional { get; set; }

        public PropertyViewModel(Type type, string name)
        {
            Type = type;
            Name = name;
            Id = Guid.NewGuid().ToString();

            //if (html.ViewData.ModelState.TryGetValue(name, out modelState))
            //{
            //    if (modelState.Value != null)
            //        Value = modelState.Value.AttemptedValue;
            //}
            GetCustomAttributes = () => new object[] { };
        }

        public string Id { get; set; }
        public Type Type { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public object Value { get; set; }


        public Func<IEnumerable<object>> GetCustomAttributes { get; set; }
        public T GetAttribute<T>()
        {
            return GetCustomAttributes().OfType<T>().SingleOrDefault();
        }

        public bool Readonly { get; set; }
        public bool Disabled { get; set; }


        public IEnumerable Choices { get; set; }
        public IEnumerable Suggestions { get; set; }

        public bool IsHidden { get; set; }
    }

    public static class Extensions
    {
        public static U Maybe<T, U>(this T t, Func<T, U> f) where T : class
        {
            return (t == null) ? default(U) : f(t);
        }
        public static bool HasAttribute<TAtt>(this PropertyViewModel propertyViewModel)
        {
            return propertyViewModel.GetCustomAttributes().OfType<TAtt>().Any();
        }
    }
}