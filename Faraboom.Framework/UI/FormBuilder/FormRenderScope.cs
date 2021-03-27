using System;

namespace Faraboom.Framework.UI.FormBuilder
{
    public class FormRenderScope : IDisposable {
        public IFormBuilderHtmlHelper HtmlHelper { get; set; }
        public FormViewModel FormViewModel { get; private set; }

        public FormRenderScope(IFormBuilderHtmlHelper htmlHelper, FormViewModel formViewModel)
        {
            HtmlHelper = htmlHelper;
            FormViewModel = formViewModel;
        }

        public FormRenderScope Render()
        {
            RenderStart();
            RenderActionInputs();
            RenderButtons();
            return this;
        }

        public FormRenderScope RenderButtons()
        {
            HtmlHelper.RenderPartial("FormBuilder/Form.Actions", this.FormViewModel);
            return this;
        }

        public FormRenderScope RenderActionInputs()
        {
            foreach (var input in FormViewModel.Inputs)
            {
                HtmlHelper.RenderPartial("FormBuilder/Form.Property", input);
            }
            return this;
        }

        public FormRenderScope RenderStart()
        {
            HtmlHelper.RenderPartial("FormBuilder/Form.Start", this.FormViewModel);
            return this;
        }


        public void Dispose()
        {
            HtmlHelper.RenderPartial("FormBuilder/Form.Close", this.FormViewModel);
        }
   
    }
}