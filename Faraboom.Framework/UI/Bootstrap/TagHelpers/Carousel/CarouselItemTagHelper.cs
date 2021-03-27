using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Faraboom.Framework.UI.Bootstrap.TagHelpers.Carousel
{
    [DataAnnotation.Injectable]
    [HtmlTargetElement("frb-carousel-item")]
    public class CarouselItemTagHelper : TagHelper<CarouselItemTagHelper, CarouselItemTagHelperService>
    {
        [HtmlAttributeName("frb-active")]
        public bool? Active { get; set; }

        [HtmlAttributeName("frb-src")]
        public string Src { get; set; }

        [HtmlAttributeName("frb-alt")]
        public string Alt { get; set; }

        [HtmlAttributeName("frb-caption-title")]
        public string CaptionTitle { get; set; }

        [HtmlAttributeName("frb-caption")]
        public string Caption { get; set; }

        public CarouselItemTagHelper(CarouselItemTagHelperService tagHelperService, IOptions<MvcViewOptions> optionsAccessor)
            : base(tagHelperService, optionsAccessor)
        {

        }
    }
}
