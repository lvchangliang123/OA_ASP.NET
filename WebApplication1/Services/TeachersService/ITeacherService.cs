using System.Threading.Tasks;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Services.TeachersService
{
    public interface ITeacherService
    {
        Task<PagedResultDto<Teacher>> GetPagedTeacherList(GetTeacherInput input);
    }
}
