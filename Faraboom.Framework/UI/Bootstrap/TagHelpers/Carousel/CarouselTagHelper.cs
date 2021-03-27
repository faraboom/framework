using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Carousel
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-carousel")]
    public class CarouselTagHelper : TagHelper<CarouselTagHelper, CarouselTagHelperService>
    {
        [HtmlAttributeName("frb-id")]
        public string Id { get; set; }

        [HtmlAttributeName("frb-crossfade")]
        public bool? Crossfade { get; set; }

        [HtmlAttributeName("frb-controls")]
        public bool? Controls { get; set; }

        [HtmlAttributeName("frb-indicators")]
        public bool? Indicators { get; set; }

        public CarouselTagHelper(CarouselTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
