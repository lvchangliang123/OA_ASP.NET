using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<Student>> GetPaginatedResult(int currentPage,string searchString,string sortBy, int pageSize = 3)
        {
            var query=_studentRepository.GetAll();
            if (!string.IsNullOrEmpty(searchString)) {
                query = query.Where(s => s.Name.Contains(searchString) || s.Email.Contains(searchString));
            }
            query = query.OrderBy(sortBy);
            return await query.Skip((currentPage-1)*pageSize).Take(pageSize).AsNoTracking().ToListAsync();
        }
    }
}
