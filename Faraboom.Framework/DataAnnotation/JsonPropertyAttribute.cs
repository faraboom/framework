using System;

namespace Faraboom.Framework.DataAnnotation
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class JsonPropertyAttribute : Attribute
    {
        public string ReadName { get; set; }
        public string WriteName { get; set; }
    }
}