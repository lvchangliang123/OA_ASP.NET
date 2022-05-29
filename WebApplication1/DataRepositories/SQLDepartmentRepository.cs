using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Infrastructure;
using WebApplication1.Models;

namespace WebApplication1.DataRepositories
{
    public class SQLDepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext dbContext;
        public SQLDepartmentRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Department Delete(int id)
        {
            Department department = dbContext.Departments.Find(id);
            if (department != null) {
                dbContext.Departments.Remove(department);
                dbContext.SaveChanges();
            }
            return department;
        }

        public System.Linq.IQueryable<Department> GetAllDepartments()
        {
            return dbContext.Departments;
        }

        public Department GetDepartmentById(int id)
        {
            return dbContext.Departments.Find(id);
        }

        public Department Insert(Department department)
        {
            dbContext.Departments.Add(department);
            dbContext.SaveChanges();
            return department;
        }

        public async Task<int> InsertAsync(Department department)
        {
            await dbContext.Departments.AddAsync(department);
            return dbContext.SaveChanges();
        }

        public Department Update(Department department)
        {
            var updateDepartment = dbContext.Departments.Attach(department);
            updateDepartment.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            dbContext.SaveChanges();
            return department;
        }
    }
}
