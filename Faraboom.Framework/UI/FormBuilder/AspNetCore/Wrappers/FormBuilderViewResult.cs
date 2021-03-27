using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Faraboom.Framework.UI.FormBuilder.AspNetCore.Wrappers
{
    public class FormBuilderViewFinderResult : IViewFinderResult
    {
        private readonly ViewEngineResult findPartialView;

        public FormBuilderViewFinderResult(ViewEngineResult findPartialView)
        {
            this.findPartialView = findPartialView;
        }

        public View View
        {
            get
            {
                if (findPartialView.View == null)
                    return null;
                return new FormBuilderView(findPartialView.View);
            }
        }
    }
}