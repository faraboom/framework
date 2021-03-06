namespace Faraboom.Framework.DataAccess.UnitOfWork
{
    using System;
    using System.Collections.Generic;
    using System.Data;
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
    using Microsoft.Extensions.Logging;

    public abstract class UnitOfWorkBase<TContext> : IUnitOfWorkBase
        where TContext : DbContext
    {
        protected internal UnitOfWorkBase(TContext context, IServiceProvider serviceProvider, ILogger<DataAccess> logger)
        {
            Context = context;
            ServiceProvider = serviceProvider;
            Logger = logger;
        }

        ~UnitOfWorkBase()
        {
            Dispose(false);
        }

        protected bool IsDisposed { get; set; }

        protected IServiceProvider ServiceProvider { get; }

        protected ILogger Logger { get; }

        protected TContext Context { get; set; }

        public int SaveChanges()
        {
            CheckDisposed();
            Prepare();
            return Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            CheckDisposed();
            await PrepareAsync();

            return await Context.SaveChangesAsync(cancellationToken);
        }

        public IRepository<TEntity, int> GetRepository<TEntity>()
            where TEntity : class, IEntity<TEntity, int>
            => GetRepository<TEntity, int>();

        public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : class, IEntity<TEntity, TKey>
        {
            CheckDisposed();
            var repositoryType = typeof(IRepository<TEntity, TKey>);
            if (ServiceProvider.GetService(repositoryType) is not IRepository<TEntity, TKey> repository)
            {
                throw new RepositoryNotFoundException(repositoryType.Name, $"Repository {repositoryType.Name} not found in the IOC container. Check if it is registered during startup.");
            }

            (repository as IRepositoryInjection).SetContext(Context);
            return repository;
        }

        public TRepository GetCustomRepository<TRepository>()
        {
            CheckDisposed();
            var repositoryType = typeof(TRepository);
            var repository = (TRepository)ServiceProvider.GetService(repositoryType);
            if (repository == null)
            {
                throw new RepositoryNotFoundException(repositoryType.Name, string.Format("Repository {0} not found in the IOC container. Check if it is registered during startup.", repositoryType.Name));
            }

            (repository as IRepositoryInjection).SetContext(Context);
            return repository;
        }

        public async Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<(string ParameterName, object Value)> param = null)
        {
            try
            {
                var connection = Context.Database.GetDbConnection();
                using var command = connection.CreateCommand();
                command.CommandText = sql.Replace(Constants.SchemaIdentifier, Context.Model.GetDefaultSchema());
                await connection.OpenAsync();
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 300;
                if (param != null)
                {
                    foreach (var (parameterName, value) in param)
                    {
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = parameterName;
                        parameter.Value = value;
                        command.Parameters.Add(parameter);
                    }
                }

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception exc)
            {
                Logger.LogError(exc, "Error ExecuteSqlCommandAsync: {sql}", sql);
                throw;
            }
        }

        public async Task<DataTable> SqlQueryAsync(string sql, IList<(string ParameterName, object Value)> param = null)
        {
            try
            {
                var connection = Context.Database.GetDbConnection();
                using var command = connection.CreateCommand();
                command.CommandText = sql.Replace(Constants.SchemaIdentifier, Context.Model.GetDefaultSchema());
                await connection.OpenAsync();
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 300;

                if (param != null)
                {
                    foreach (var (parameterName, value) in param)
                    {
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = parameterName;
                        parameter.Value = value;
                        command.Parameters.Add(parameter);
                    }
                }

                var dt = new DataTable();
                using var reader = await command.ExecuteReaderAsync();
                dt.Load(reader);

                return dt;
            }
            catch (Exception exc)
            {
                Logger.LogError(exc, "Error SqlQueryAsync: {sql}", sql);
                throw;
            }
        }

        public async Task<int> GetSequenceValueAsync(string sequence)
        {
            var connection = Context.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = GetSequenceCommand(sequence, Context.Model.GetDefaultSchema());
            await connection.OpenAsync();
            var value = (decimal)await command.ExecuteScalarAsync();
            return Convert.ToInt32(value);
        }

        public int GetSequenceValue(string sequence)
        {
            var connection = Context.Database.GetDbConnection();
            using var command = connection.CreateCommand();
            command.CommandText = GetSequenceCommand(sequence, Context.Model.GetDefaultSchema());
            connection.Open();
            var value = (decimal)command.ExecuteScalar();
            return Convert.ToInt32(value);
        }

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void CheckDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("The UnitOfWork is already disposed and cannot be used anymore.");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing && Context != null)
            {
                Context.Dispose();
                Context = null;
            }

            IsDisposed = true;
        }

        #endregion

        #region Private Methods

        private static string GetSequenceCommand(string sequence, string schema)
        {
            return $"SELECT {(!string.IsNullOrWhiteSpace(schema) ? $"{schema}." : string.Empty)}{sequence}.NEXTVAL FROM DUAL";
        }

        private void Prepare()
        {
            var changedEntities = Context.ChangeTracker.Entries().Where(x => x.State is EntityState.Added or EntityState.Modified);
            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                {
                    continue;
                }

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
                            {
                                property.SetValue(item.Entity, newVal, null);
                            }
                        }
                    }
                    else
                    {
                        if (item.State == EntityState.Added)
                        {
                            if (property.GetCustomAttributes(typeof(DatabaseGeneratedAttribute), false).FirstOrDefault() is DatabaseGeneratedAttribute attribute && attribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity && !string.IsNullOrWhiteSpace(attribute.SequenceName))
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
            var changedEntities = Context.ChangeTracker.Entries().Where(x => x.State is EntityState.Added or EntityState.Modified);
            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                {
                    continue;
                }

                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite);

                foreach (var property in properties)
                {
                    if (property.Name == nameof(Concurrency.IRowVersion.RowVersion))
                    {
                        var val = (long)property.GetValue(item.Entity, null);
                        property.SetValue(item.Entity, val++, null);
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        var val = property.GetValue(item.Entity, null) as string;
                        if (!string.IsNullOrWhiteSpace(val))
                        {
                            var newVal = val.NormalizePersian();
                            if (newVal != val)
                            {
                                property.SetValue(item.Entity, newVal, null);
                            }
                        }
                    }
                    else
                    {
                        if (item.State == EntityState.Added)
                        {
                            if (property.GetCustomAttributes(typeof(DatabaseGeneratedAttribute), false).FirstOrDefault() is DatabaseGeneratedAttribute attribute && attribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity && !string.IsNullOrWhiteSpace(attribute.SequenceName))
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
