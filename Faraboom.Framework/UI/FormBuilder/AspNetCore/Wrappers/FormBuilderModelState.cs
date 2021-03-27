using System.Linq;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Faraboom.Framework.UI.FormBuilder.AspNetCore.Wrappers
{

    static class MvcToFormBuilderMappingExtensions
    {
        public static FormBuilderModelStateErrors ToFormBuilderModelStateErrors(this ModelErrorCollection mvcErrors)
        {
            return new FormBuilderModelStateErrors(mvcErrors.Select(e => new FormBuilderModelStateError
            {
                ErrorMessage = e.ErrorMessage
            }));
        }
    }
}