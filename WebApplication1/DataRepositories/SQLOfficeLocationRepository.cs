using WebApplication1.Infrastructure;
using WebApplication1.Infrastructure.Repositories;
using WebApplication1.Models;

namespace WebApplication1.DataRepositories
{
    public class SQLOfficeLocationRepository : RepositoryBase<OfficeLocation, int>
    {
        public SQLOfficeLocationRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
