using WebApplication1.Infrastructure;
using WebApplication1.Infrastructure.Repositories;
using WebApplication1.Models;

namespace WebApplication1.DataRepositories
{
    public class SQLCourseAssignmentRepository : RepositoryBase<CourseAssignment, int>
    {
        public SQLCourseAssignmentRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
