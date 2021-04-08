using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Faraboom.Framework.Core;
using Faraboom.Framework.DataAccess.Entities;
using Faraboom.Framework.DataAccess.Exceptions;
using Faraboom.Framework.DataAccess.Repositories;
using Faraboom.Framework.DataAnnotation.Schema;

using Microsoft.EntityFrameworkCore;

namespace Faraboom.Framework.DataAccess.UnitOfWork
{
    public abstract class UnitOfWorkBase<TContext> : IUnitOfWorkBase
        where TContext : DbContext
    {
        protected TContext context;
        protected readonly IServiceProvider serviceProvider;

        protected internal UnitOfWorkBase(TContext context, IServiceProvider serviceProvider)
        {
            this.context = context;
            this.serviceProvider = serviceProvider;
        }

        public int SaveChanges()
        {
            CheckDisposed();
            Prepare();
            return context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            CheckDisposed();
            await PrepareAsync();

            return await context.SaveChangesAsync(cancellationToken);
        }

        public IRepository<TEntity, int> GetRepository<TEntity>() where TEntity : class, IEntity<TEntity, int> => GetRepository<TEntity, int>();
        public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class, IEntity<TEntity, TKey>
        {
            CheckDisposed();
            var repositoryType = typeof(IRepository<TEntity, TKey>);
            var repository = serviceProvider.GetService(repositoryType) as IRepository<TEntity, TKey>;
            if (repository == null)
                throw new RepositoryNotFoundException(repositoryType.Name, $"Repository {repositoryType.Name} not found in the IOC container. Check if it is registered during startup.");

            (repository as IRepositoryInjection).SetContext(context);
            return repository;
        }

        public TRepository GetCustomRepository<TRepository>()
        {
            CheckDisposed();
            var repositoryType = typeof(TRepository);
            var repository = (TRepository)serviceProvider.GetService(repositoryType);
            if (repository == null)
                throw new RepositoryNotFoundException(repositoryType.Name, String.Format("Repository {0} not found in the IOC container. Check if it is registered during startup.", repositoryType.Name));

            (repository as IRepositoryInjection).SetContext(context);
            return repository;
        }

        public async Task<int> GetSequenceValueAsync(string sequence)
        {
            var connection = context.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = getSequenceCommand(sequence, context.Model.GetDefaultSchema());
            await connection.OpenAsync();
            var value = (decimal)await command.ExecuteScalarAsync();
            return Convert.ToInt32(value);
        }

        public int GetSequenceValue(string sequence)
        {
            var connection = context.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = getSequenceCommand(sequence, context.Model.GetDefaultSchema());
            connection.Open();
            var value = (decimal)command.ExecuteScalar();
            return Convert.ToInt32(value);
        }

        #region IDisposable Implementation

        protected bool isDisposed;

        protected void CheckDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException("The UnitOfWork is already disposed and cannot be used anymore.");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed && disposing && context != null)
            {
                context.Dispose();
                context = null;
            }
            isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWorkBase()
        {
            Dispose(false);
        }

        #endregion

        #region Private Methods

        private string getSequenceCommand(string sequence, string schema)
        {
            return $"SELECT {(!string.IsNullOrWhiteSpace(schema) ? $"{schema}." : "")}{sequence}.NEXTVAL FROM DUAL";
        }

        private void Prepare()
        {
            var changedEntities = context.ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                    continue;

                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite);

                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        var val = property.GetValue(item.Entity, null) as string;
                        if (!string.IsNullOrWhiteSpace(val))
                        {
                            var newVal = val.NormalizePersian();
                            if (newVal != val)
                                property.SetValue(item.Entity, newVal, null);
                        }
                    }
                    else
                    {
                        if (item.State == EntityState.Added)
                        {
                            var attribute = property.GetCustomAttributes(typeof(DatabaseGeneratedAttribute), false).FirstOrDefault() as DatabaseGeneratedAttribute;
                            if (attribute != null && attribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity && !string.IsNullOrWhiteSpace(attribute.SequenceName))
                            {
                                var val = GetSequenceValue(attribute.SequenceName);
                                property.SetValue(item.Entity, val, null);
                            }
                        }
                    }
                }
            }
        }

        private async Task PrepareAsync()
        {
            var changedEntities = context.ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                    continue;

                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite);

                foreach (var property in properties)
                {
                    if (property.Name == nameof(Concurrency.IRowVersion.RowVersion))
                    {
                        var val = (short)property.GetValue(item.Entity, null);
                        property.SetValue(item.Entity, val++, null);
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        var val = property.GetValue(item.Entity, null) as string;
                        if (!string.IsNullOrWhiteSpace(val))
                        {
                            var newVal = val.NormalizePersian();
                            if (newVal != val)
                                property.SetValue(item.Entity, newVal, null);
                        }
                    }
                    else
                    {
                        if (item.State == EntityState.Added)
                        {
                            var attribute = property.GetCustomAttributes(typeof(DatabaseGeneratedAttribute), false).FirstOrDefault() as DatabaseGeneratedAttribute;
                            if (attribute != null && attribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity && !string.IsNullOrWhiteSpace(attribute.SequenceName))
                            {
                                var val = await GetSequenceValueAsync(attribute.SequenceName);
                                property.SetValue(item.Entity, val, null);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}