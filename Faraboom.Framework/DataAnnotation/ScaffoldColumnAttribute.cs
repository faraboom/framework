using System;

namespace Faraboom.Framework.DataAnnotation
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ScaffoldColumnAttribute : System.ComponentModel.DataAnnotations.ScaffoldColumnAttribute
    {
        public ScaffoldColumnAttribute(bool scaffold)
            :base(scaffold)
        {
        }
    }
}
