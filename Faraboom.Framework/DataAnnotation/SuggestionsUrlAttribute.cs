using System;

namespace Faraboom.Framework.DataAnnotation
{
    public sealed class SuggestionsUrlAttribute : Attribute
    {
        public string Url { get; }

        public SuggestionsUrlAttribute(string url)
        {
            Url = url;
        }
    }
}