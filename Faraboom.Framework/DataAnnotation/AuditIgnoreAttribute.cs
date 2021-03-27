using System;

namespace Faraboom.Framework.DataAnnotation
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false)]
    public sealed class AuditIgnoreAttribute : Attribute
    {
    }
}