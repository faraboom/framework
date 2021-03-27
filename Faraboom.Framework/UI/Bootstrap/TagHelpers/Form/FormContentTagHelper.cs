using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Form
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-form-content", TagStructure = TagStructure.WithoutEndTag)]
    public class FormContentTagHelper : TagHelper<FormContentTagHelper, FormContentTagHelperService>
    {
        public FormContentTagHelper(FormContentTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
