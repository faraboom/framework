using Faraboom.Framework.Data;

namespace Faraboom.Framework.DataAnnotation.Schema
{
    public sealed class ForeignKeyAttribute : System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute
    {
        public ForeignKeyAttribute(string name)
            : base(DbProviderFactories.GetFactory.GetObjectName(name))
        {
        }
    }
}