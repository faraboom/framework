namespace Faraboom.Framework.DataAccess.Repositories
{
    using Faraboom.Framework.DataAccess.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class GenericEntityRepository<TEntity, TKey> : EntityRepositoryBase<DbContext, TEntity, TKey>
        where TEntity : class, IEntity<TEntity, TKey>, new()
    {
        public GenericEntityRepository(ILogger<DataAccess> logger)
            : base(logger, null)
        {
        }
    }
}
