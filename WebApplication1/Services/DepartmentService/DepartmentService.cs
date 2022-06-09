using System.Threading.Tasks;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Infrastructure.Repositories;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Services.DepartmentService
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository<Department, int> _departmentRepository;
        public DepartmentService(IRepository<Department, int> departmentRepository)
        {
            _departmentRepository= departmentRepository;
        }
        public async Task<PagedResultDto<Department>> GetPagedDepartmentsList(GetDepartmentInput input)
        {
            var query=_departmentRepository.GetAll();
            if (!string.IsNullOrEmpty(input.FilterText)) {
                query = query.Where(s => s.Name.Contains(input.FilterText));
            }
            var count=query.Count();
            query = query.OrderBy(input.Sorting).Skip((input.CurrentPage - 1) * input.MaxResultCount).Take(input.MaxResultCount);
            var models = await query.Include(d => d.Administrator).AsNoTracking().ToListAsync();
            var dtos = new PagedResultDto<Department> {
                TotalCount = count,
                CurrentPage = input.CurrentPage,
                MaxResultCount = input.MaxResultCount,
                Data = models,
                FilterText = input.FilterText,
                Sorting = input.Sorting,
            };
            return dtos;
        }
    }
}
