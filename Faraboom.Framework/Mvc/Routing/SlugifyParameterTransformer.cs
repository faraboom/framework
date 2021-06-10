namespace Faraboom.Framework.Mvc.Routing
{
    using Faraboom.Framework.Core;
    using Microsoft.AspNetCore.Routing;

    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value) => (value as string).Slugify();
    }
}
