using System.Threading.Tasks;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Services.DepartmentService
{
    public interface IDepartmentService
    {
        Task<PagedResultDto<Department>> GetPagedDepartmentsList(GetDepartmentInput input);
    }
}
