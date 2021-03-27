using System.Threading.Tasks;

using Faraboom.Framework.Mvc.ViewFeatures;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers
{
    public abstract class TagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        private readonly IOptions<MvcViewOptions> optionsAccessor;

        [HtmlAttributeName("frb-for")]
        public virtual ModelExpression For { get; set; }

        [HtmlAttributeName("frb-name")]
        public virtual string Name { get; set; }

        [HtmlAttributeName("frb-value")]
        public virtual object Value { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private string elementName;
        protected string ElementName => elementName ??= NameAndIdProvider.GetFullHtmlFieldName(ViewContext, Name ?? For.Name);

        private string elementId;
        protected string ElementId => elementId ??= NameAndIdProvider.CreateSanitizedId(ViewContext, ElementName, optionsAccessor.Value.HtmlHelperOptions.IdAttributeDotReplacement);

        protected TagHelper(IOptions<MvcViewOptions> optionsAccessor)
        {
            this.optionsAccessor = optionsAccessor;
        }
    }

    public abstract class TagHelper<TTagHelper, TService> : TagHelper
        where TTagHelper : TagHelper<TTagHelper, TService>
        where TService : class, ITagHelperService<TTagHelper>
    {
        protected TagHelper(TService service, IOptions<MvcViewOptions> optionsAccessor)
            : base(optionsAccessor)
        {
            Service = service;
            (Service as TagHelperService<TTagHelper>).TagHelper = (TTagHelper)this;
        }

        protected TService Service { get; }

        public override int Order => Service.Order;

        public virtual bool DisplayRequiredSymbol { get; set; } = true;

        public override void Init(TagHelperContext context)
        {
            Service.Init(context);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Service.Process(context, output);
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            return Service.ProcessAsync(context, output);
        }
    }
}
