using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Faraboom.Framework.UI.FormBuilder.ModelBinding;

namespace Faraboom.Framework.UI.FormBuilder
{
    public static class FB
    {
        static FB()
        {
            GetPropertyViewModels = GetPropertyViewModelsUsingReflection;
        }

        public static Func<object, Type, IEnumerable<PropertyViewModel>> GetPropertyViewModels { get; set; }

        public static IStringEncoder StringEncoder { get; set; } = new NonEncodingStringencoder();


        public static IEnumerable<PropertyViewModel> PropertiesFor(object model, Type fallbackModelType = null)
        {
            fallbackModelType = fallbackModelType ?? model.GetType();
            return GetPropertyViewModels(model, fallbackModelType);
        }

        public static PropertyViewModel PropertyViewModel(Type type, string name, object value)
        {
            return new PropertyViewModel(type, name) { Value = value };
        }

        public static IEnumerable<PropertyViewModel> GetPropertyViewModelsUsingReflection(object model, Type fallbackModelType)
        {
            if (model is PropertyViewModel vm)
            {
                yield return vm;
                yield break;
            }
            var type = model != null ? model.GetType() : fallbackModelType;

            var typeVm = new PropertyViewModel(typeof(string), "__type")
            {
                DisplayName = "",
                IsHidden = true,
                Value = StringEncoder
            };

            yield return typeVm;

            var typeProperties = type.GetTypeInfo().DeclaredProperties;

            if (type.GetTypeInfo().BaseType != null)
            {
                var baseTypeProperties = type.GetTypeInfo().BaseType.GetTypeInfo().DeclaredProperties;
                typeProperties = typeProperties.Union(baseTypeProperties);
            }

            var properties = typeProperties as IList<PropertyInfo> ?? typeProperties.ToList();


            foreach (var property in properties)
            {
                if (properties.Any(p => p.Name + "_choices" == property.Name))
                {
                    continue; //skip this is it is choice
                }

                if (properties.Any(p => p.Name + "_choices" == property.Name))
                {
                    continue; //skip this is it is choice
                }

                if (properties.Any(p => p.Name + "_show" == property.Name))
                {
                    continue; //skip this is it is show
                }


                if (!(type.GetTypeInfo().GetDeclaredMethod(property.Name + "_show")?.Invoke(model, null) as bool? ?? true))
                    continue;
                if (!(type.GetTypeInfo().GetDeclaredProperty(property.Name + "_show")?.GetValue(model) as bool? ?? true))
                    continue;


                var inputVm = new PropertyViewModel(model, property);
                PropertyInfo choices = properties.SingleOrDefault(p => p.Name == property.Name + "_choices");
                if (choices != null)
                {
                    inputVm.Choices = (IEnumerable)choices.GetMethod.Invoke(model, null);
                }
                PropertyInfo suggestions = properties.SingleOrDefault(p => p.Name == property.Name + "_suggestions");
                if (suggestions != null)
                {
                    inputVm.Suggestions = (IEnumerable)suggestions.GetMethod.Invoke(model, null);
                }

                yield return inputVm;
            }
        }
    }
}