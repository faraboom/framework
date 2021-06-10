namespace Faraboom.Framework.DataAnnotation.Schema
{
    using Faraboom.Framework.Data;

    public sealed class InversePropertyAttribute : System.ComponentModel.DataAnnotations.Schema.InversePropertyAttribute
    {
        public InversePropertyAttribute(string property)
            : base(DbProviderFactories.GetFactory.GetObjectName(property))
        {
        }
    }
}