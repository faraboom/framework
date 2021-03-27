using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.ListGroup
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-list-group")]
    public class ListGroupTagHelper : TagHelper<ListGroupTagHelper, ListGroupTagHelperService>
    {
        [HtmlAttributeName("frb-flush")]
        public bool? Flush { get; set; }

        public ListGroupTagHelper(ListGroupTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
