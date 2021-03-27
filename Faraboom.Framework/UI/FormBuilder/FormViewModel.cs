using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Faraboom.Framework.Core;
using Faraboom.Framework.UI.FormBuilder.Components;

namespace Faraboom.Framework.UI.FormBuilder
{
    public class FormViewModel : IHasDisplayName
    {
        public FormViewModel(MethodInfo mi, string actionUrl)  
        {
            var inputs = new List<PropertyViewModel>();
            foreach (var pi in mi.GetParameters())
            {
                if (pi.GetCustomAttributes(true).Any(x => x is FormModelAttribute))
                {
                    inputs.AddRange(pi.ParameterType.GetTypeInfo().DeclaredProperties
                                        .Select(pi2 => new PropertyViewModel(pi, pi2).Then(p => p.Name = pi.Name + "." + p.Name)));
                }
                else
                {
                    inputs.Add(new PropertyViewModel(pi));
                }
            }

            Inputs = inputs;
            Buttons.Add(new PropertyViewModel(typeof(SubmitButton), "") { Value = new SubmitButton()});
            this.DisplayName = mi.Name.Sentencise(true);
            ExcludePropertyErrorsFromValidationSummary = true;


            this.ActionUrl = actionUrl;
        }

        public FormViewModel()
        {
            this.DisplayName = "";
            ShowValidationSummary = true;
            ExcludePropertyErrorsFromValidationSummary = true;
            RenderAntiForgeryToken = Defaults.RenderAntiForgeryToken;
        }

        public string ActionUrl { get; set; }
        
        public string Method { get; set; }
        
        public string EncType { get; set; } = "multipart/form-data";
        
        public IList<PropertyViewModel> Inputs { get; set; } = new List<PropertyViewModel>();
        
        public IList<PropertyViewModel> Buttons { get; set; } = new List<PropertyViewModel>();


        public string DisplayName { get; set; }

        public bool ExcludePropertyErrorsFromValidationSummary { get; set; }

        public bool ShowValidationSummary { get; set; }
     
        public bool RenderAntiForgeryToken { get; set; }

        public string AdditionalClasses { get; set; }
    }
}

