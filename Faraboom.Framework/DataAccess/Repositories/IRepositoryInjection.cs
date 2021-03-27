using Microsoft.EntityFrameworkCore;

namespace Faraboom.Framework.DataAccess.Repositories
{
    public interface IRepositoryInjection
    {
        IRepositoryInjection SetContext(DbContext context);
    }
}