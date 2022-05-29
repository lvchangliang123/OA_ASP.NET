using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Dtos;

namespace WebApplication1.Students
{
    public interface IStudentService
    {
        Task<PagedResultDto<Student>> GetPaginatedResult(GetStudentInput input);
    }
}
