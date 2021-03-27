using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Faraboom.Framework.DataAccess.Repositories
{
    public abstract class RepositoryBase<TContext> : IRepositoryInjection where TContext : DbContext
    {
        protected ILogger Logger { get; }
        protected TContext Context { get; private set; }

        protected RepositoryBase(ILogger<DataAccess> logger, TContext context)
        {
            Logger = logger;
            Context = context;
        }


        public IRepositoryInjection SetContext(DbContext context)
        {
            Context = (TContext)context;
            return this;
        }
    }
}