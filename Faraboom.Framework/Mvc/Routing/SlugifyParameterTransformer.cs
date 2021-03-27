using Faraboom.Framework.Core;

using Microsoft.AspNetCore.Routing;

namespace Faraboom.Framework.Mvc.Routing
{
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value) => (value as string).Slugify();
    }
}
