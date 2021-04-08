using Faraboom.Framework.Data;
using Faraboom.Framework.DataAnnotation.Schema;

namespace Faraboom.Framework.DataAccess.Concurrency
{
    public interface IRowVersion
    {
        [System.ComponentModel.DataAnnotations.ConcurrencyCheck]
        [Column(nameof(RowVersion), DataType.Short)]
        short RowVersion { get; set; }
    }
}
