namespace Faraboom.Framework.DataAnnotation.Schema
{
    using Faraboom.Framework.Data;

    public sealed class ColumnAttribute : System.ComponentModel.DataAnnotations.Schema.ColumnAttribute
    {
        public ColumnAttribute(string name, DataType dataType)
            : base(DbProviderFactories.GetFactory.GetObjectName(name, pluralize: false))
        {
            TypeName = DbProviderFactories.GetFactory.GetColumnTypeName(dataType);
            DataType = dataType;
        }

        public DataType DataType { get; }
    }
}
