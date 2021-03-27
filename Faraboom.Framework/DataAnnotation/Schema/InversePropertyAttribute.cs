using Faraboom.Framework.Data;

namespace Faraboom.Framework.DataAnnotation.Schema
{
    public sealed class InversePropertyAttribute : System.ComponentModel.DataAnnotations.Schema.InversePropertyAttribute
    {
        public InversePropertyAttribute(string property)
            : base(DbProviderFactories.GetFactory.GetObjectName(property))
        {
        }
    }
}