using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Faraboom.Framework.UI.FormBuilder.AspNetCore.Wrappers
{
    public class FormBuilderView : View
    {
        private readonly IView view;

        public FormBuilderView(IView view)
        {
            this.view = view;
        }
    }
}