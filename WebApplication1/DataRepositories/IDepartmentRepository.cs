using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.DataRepositories
{
    public interface IDepartmentRepository
    {
        Department GetDepartmentById(int id);
        System.Linq.IQueryable<Department> GetAllDepartments();
        Department Insert(Department department);
        Task<int> InsertAsync(Department department);
        Department Update(Department department);
        Department Delete(int id);
    }
}
