using System.Threading.Tasks;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Courses
{
    public interface ICourseService
    {
        Task<PagedResultDto<Course>> GetPaginatedResult(GetCourseInput input);

    }
}
