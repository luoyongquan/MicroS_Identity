using Microsoft.EntityFrameworkCore;

namespace Core.EntityFrameworkCore.Repositories
{
    public interface IRepositoryWithDbContext
    {
        DbContext GetDbContext();
    }
}