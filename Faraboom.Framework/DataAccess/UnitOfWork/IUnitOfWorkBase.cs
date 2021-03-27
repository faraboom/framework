using System;
using System.Threading;
using System.Threading.Tasks;
using Faraboom.Framework.DataAccess.Entities;
using Faraboom.Framework.DataAccess.Repositories;

namespace Faraboom.Framework.DataAccess.UnitOfWork
{
    public interface IUnitOfWorkBase : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        IRepository<TEntity, int> GetRepository<TEntity>() where TEntity : class, IEntity<TEntity, int>;
        IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class, IEntity<TEntity, TKey>;
        TRepository GetCustomRepository<TRepository>();

        Task<int> GetSequenceValueAsync(string sequence);
        int GetSequenceValue(string sequence);
    }
}