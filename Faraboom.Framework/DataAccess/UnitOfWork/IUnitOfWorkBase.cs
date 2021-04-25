using System;
using System.Collections.Generic;
using System.Data;
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

        Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<(string parameterName, object value)> param = null);
        Task<DataTable> SqlQueryAsync(string sql, IList<(string parameterName, object value)> param = null);

        Task<int> GetSequenceValueAsync(string sequence);
        int GetSequenceValue(string sequence);
    }
}