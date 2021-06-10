namespace Faraboom.Framework.DataAnnotation.Schema
{
    using Faraboom.Framework.Data;

    public sealed class ForeignKeyAttribute : System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute
    {
        public ForeignKeyAttribute(string name)
            : base(DbProviderFactories.GetFactory.GetObjectName(name))
        {
        }
    }
}