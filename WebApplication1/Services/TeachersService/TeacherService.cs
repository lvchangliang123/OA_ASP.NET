using System.Threading.Tasks;
using WebApplication1.Dtos;
using WebApplication1.Infrastructure.Repositories;
using WebApplication1.Models;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Services.TeachersService
{
    public class TeacherService : ITeacherService
    {
        private readonly IRepository<Teacher, int> _teacherRepository;
        public TeacherService(IRepository<Teacher, int> teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task<PagedResultDto<Teacher>> GetPagedTeacherList(GetTeacherInput input)
        {
            var query=_teacherRepository.GetAll();
            if (!string.IsNullOrEmpty(input.FilterText)) {
                query = query.Where(s => s.Name.Contains(input.FilterText));
            }
            var count=query.Count();
            query = query.OrderBy(input.Sorting).Skip((input.CurrentPage - 1) * input.MaxResultCount).Take(input.MaxResultCount);

            //加载数据到内存中
            var models = await query.Include(t => t.OfficeLocation).Include(t => t.CourseAssignments).ThenInclude(ca => ca.Course).ThenInclude(c => c.StudentCourses).ThenInclude(sc => sc.Student).Include(sc => sc.CourseAssignments).ThenInclude(ca => ca.Course).ThenInclude(c => c.Department).AsNoTracking().ToListAsync();

            var dtos = new PagedResultDto<Teacher> {
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
