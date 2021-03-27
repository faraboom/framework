using Faraboom.Framework.DataAnnotation.Schema;
using Microsoft.EntityFrameworkCore;

namespace Faraboom.Framework.DataAccess.Entities
{
    public interface IEntity<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        [NotMapped]
        TKey Id { get; set; }
    }
}