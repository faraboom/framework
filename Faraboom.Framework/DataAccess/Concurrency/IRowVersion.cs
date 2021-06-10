namespace Faraboom.Framework.DataAccess.Concurrency
{
    using Faraboom.Framework.Data;
    using Faraboom.Framework.DataAnnotation.Schema;

    public interface IRowVersion
    {
        [System.ComponentModel.DataAnnotations.ConcurrencyCheck]
        [Column(nameof(RowVersion), DataType.Short)]
        long RowVersion { get; set; }
    }
}
