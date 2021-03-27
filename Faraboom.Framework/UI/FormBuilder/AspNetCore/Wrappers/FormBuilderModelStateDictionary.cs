using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Faraboom.Framework.UI.FormBuilder.AspNetCore.Wrappers
{
    public class FormBuilderModelStateDictionary : IModelStateDictionary
    {
        private readonly ModelStateDictionary modelState;

        public FormBuilderModelStateDictionary(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public bool TryGetValue(string key, out ModelState modelState)
        {
            ModelStateEntry ms;
            var result = this.modelState.TryGetValue(key, out ms);
            if (result)
            {
                modelState = new ModelState(
                    ms?.Errors?.ToFormBuilderModelStateErrors(),
                    new FormBuilderModelStateValue(ms?.AttemptedValue)
                    );
            }
            else
            {
                modelState = null;
            }
            return result;
        }

        public ModelState this[string key]
        {
            get
            {
                var value = modelState[key];
                if (value == null)
                    return null;
                return new ModelState(
                    value?.Errors?.ToFormBuilderModelStateErrors(),
                    new FormBuilderModelStateValue(value?.AttemptedValue)
                    );
            }
        }

        public bool ContainsKey(string key)
        {
            return modelState.ContainsKey(key);
        }

        public bool IsValid => modelState.IsValid;
    }
}