using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Dtos;
using WebApplication1.Infrastructure.Repositories;
using WebApplication1.Models;

namespace WebApplication1.Students
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<Student, int> _studentRepository;
        public StudentService(IRepository<Student, int> studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public async Task<PagedResultDto<Student>> GetPaginatedResult(GetStudentInput input)
        {
            var query=_studentRepository.GetAll();
            if (!string.IsNullOrEmpty(input.FilterText)) {
                query = query.Where(s => s.Name.Contains(input.FilterText) || s.Email.Contains(input.FilterText));
            }
            var count=query.Count();
            query = query.OrderBy(input.Sorting).Skip((input.CurrentPage - 1) * input.MaxResultCount).Take(input.MaxResultCount);
            var models=await query.AsNoTracking().ToListAsync();
            var dtos = new PagedResultDto<Student> {
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
