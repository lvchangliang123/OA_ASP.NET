using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Students
{
    public interface IStudentService
    {
        Task<List<Student>> GetPaginatedResult(int currentPage, string searchString, string sortBy, int pageSize=3);
    }
}
