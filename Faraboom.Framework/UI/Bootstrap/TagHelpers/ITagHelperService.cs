using System.Threading.Tasks;

using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers
{
    public interface ITagHelperService<TTagHelper>
        where TTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        TTagHelper TagHelper { get; }

        int Order { get; }

        void Init(TagHelperContext context);

        void Process(TagHelperContext context, TagHelperOutput output);

        Task ProcessAsync(TagHelperContext context, TagHelperOutput output);
    }
}