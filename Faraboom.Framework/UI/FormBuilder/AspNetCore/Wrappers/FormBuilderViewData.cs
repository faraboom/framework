using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Faraboom.Framework.UI.FormBuilder.AspNetCore.Wrappers
{
    public class FormBuilderViewData : ViewData
    {
        public FormBuilderViewData(ViewDataDictionary viewData) 
            : base(new FormBuilderModelStateDictionary(viewData.ModelState), viewData.Model)
        {
        }
    }
}