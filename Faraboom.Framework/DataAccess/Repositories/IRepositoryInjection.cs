namespace Faraboom.Framework.DataAccess.Repositories
{
    using Microsoft.EntityFrameworkCore;

    public interface IRepositoryInjection
    {
        IRepositoryInjection SetContext(DbContext context);
    }
}