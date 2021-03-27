using Microsoft.EntityFrameworkCore;

namespace Faraboom.Framework.DataAccess.UnitOfWork
{
    [DataAnnotation.Injectable]
    public interface IUnitOfWorkProvider
    {
        IUnitOfWork CreateUnitOfWork(bool trackChanges = true, bool enableLogging = false);
        IUnitOfWork CreateUnitOfWork<TEntityContext>(bool trackChanges = true, bool enableLogging = false) where TEntityContext : DbContext;
    }
}
