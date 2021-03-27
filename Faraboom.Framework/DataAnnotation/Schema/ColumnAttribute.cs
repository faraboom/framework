using Faraboom.Framework.Data;

namespace Faraboom.Framework.DataAnnotation.Schema
{
    public sealed class ColumnAttribute : System.ComponentModel.DataAnnotations.Schema.ColumnAttribute
    {
        public ColumnAttribute(string name, DataType dataType)
            : base(DbProviderFactories.GetFactory.GetObjectName(name, pluralize: false))
        {
            TypeName = DbProviderFactories.GetFactory.GetColumnTypeName(dataType);
        }
    }
}