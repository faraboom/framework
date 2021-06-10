namespace Faraboom.Framework.DataAccess.Entities
{
    using Faraboom.Framework.DataAnnotation.Schema;
    using Microsoft.EntityFrameworkCore;

    public interface IEntity<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        [NotMapped]
        TKey Id { get; set; }
    }
}