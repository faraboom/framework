using System;
using Microsoft.Extensions.Logging;
using Faraboom.Framework.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Faraboom.Framework.DataAnnotation;

namespace Faraboom.Framework.DataAccess.UnitOfWork
{
    [ServiceLifetime(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped)]
    public class UnitOfWorkProvider : IUnitOfWorkProvider
    {
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;

        public UnitOfWorkProvider()
        {
        }

        public UnitOfWorkProvider(ILogger<DataAccess> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }


        public IUnitOfWork CreateUnitOfWork(bool trackChanges = true, bool enableLogging = false)
        {
            var context = serviceProvider.GetService(typeof(IEntityContext)) as DbContext;

            if (!trackChanges)
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return new UnitOfWork(context, serviceProvider);
        }

        public IUnitOfWork CreateUnitOfWork<TEntityContext>(bool trackChanges = true, bool enableLogging = false) 
            where TEntityContext : DbContext
        {
            var context = serviceProvider.GetService(typeof(IEntityContext)) as TEntityContext;

            if (!trackChanges)
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return new UnitOfWork(context, serviceProvider);
        }
    }
}
