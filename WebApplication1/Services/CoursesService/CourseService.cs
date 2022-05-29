using System.Threading.Tasks;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Infrastructure.Repositories;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Courses
{
    public class CourseService : ICourseService
    {
        private readonly IRepository<Course, int> _courseRepository;
        public CourseService(IRepository<Course, int> courseRepository)
        {
            _courseRepository = courseRepository;
        }
        public async Task<PagedResultDto<Course>> GetPaginatedResult(GetCourseInput input)
        {
            var query = _courseRepository.GetAll();
            var count = _courseRepository.Count();
            query = query.OrderBy(input.Sorting).Skip((input.CurrentPage - 1) * input.MaxResultCount).Take(input.MaxResultCount);
            var models = await query.Include(a => a.Department).AsNoTracking().ToListAsync();
            //使用include预加载功能
            var dtos = new PagedResultDto<Course> {
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
