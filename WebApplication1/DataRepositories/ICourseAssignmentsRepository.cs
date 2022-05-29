using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication1.Infrastructure.Repositories;
using WebApplication1.Models;

namespace WebApplication1.DataRepositories
{
    public interface ICourseAssignmentsRepository:IRepository<CourseAssignment,int>
    {

    }
}
