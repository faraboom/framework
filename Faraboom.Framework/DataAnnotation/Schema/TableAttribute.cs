using Faraboom.Framework.Data;

namespace Faraboom.Framework.DataAnnotation.Schema
{
    public sealed class TableAttribute : System.ComponentModel.DataAnnotations.Schema.TableAttribute
    {
        public TableAttribute(string name, string prefix = null, bool pluralize = true)
            : base(DbProviderFactories.GetFactory.GetObjectName(name, prefix, pluralize))
        {
        }
    }
}