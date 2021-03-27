using System;

namespace Faraboom.Framework.DataAnnotation
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public sealed class InjectableAttribute : Attribute
    {
    }
}
