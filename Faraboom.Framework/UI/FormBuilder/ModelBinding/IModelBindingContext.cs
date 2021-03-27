using System;

namespace Faraboom.Framework.UI.FormBuilder.ModelBinding
{
    public interface IModelBindingContext
    {
        string ModelName { get; }
        IValueProvider ValueProvider { get; }
        object Model { get; }
        void SetModelMetadataForType(Func<object> func, Type type);
    }
}