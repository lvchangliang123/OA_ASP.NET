using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication1.Infrastructure;
using WebApplication1.Models;

namespace WebApplication1.DataRepositories
{
    public class SQLTeacherRepository : ITeacherRepository
    {
        private readonly AppDbContext dbContext;
        public SQLTeacherRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public int Count()
        {
            throw new NotImplementedException();
        }

        public int Count(Expression<Func<Teacher, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<Teacher, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Delete(Teacher entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<Teacher, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Teacher entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Expression<Func<Teacher, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Teacher FirstOrDefault(Expression<Func<Teacher, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Teacher> FirstOrDefaultAsync(Expression<Func<Teacher, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Teacher> GetAll()
        {
            return dbContext.Teachers;
        }

        public List<Teacher> GetAllList()
        {
            throw new NotImplementedException();
        }

        public List<Teacher> GetAllList(Expression<Func<Teacher, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<List<Teacher>> GetAllListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Teacher>> GetAllListAsync(Expression<Func<Teacher, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Teacher Insert(Teacher entity)
        {
            throw new NotImplementedException();
        }

        public Task<Teacher> InsertAsync(Teacher entity)
        {
            throw new NotImplementedException();
        }

        public long LongCount()
        {
            throw new NotImplementedException();
        }

        public long LongCount(Expression<Func<Teacher, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<long> LongCountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<long> LongCountAsync(Expression<Func<Teacher, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Teacher Single(Expression<Func<Teacher, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Teacher> SingleAsync(Expression<Func<Teacher, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Teacher Update(Teacher entity)
        {
            throw new NotImplementedException();
        }

        public Task<Teacher> UpdateAsync(Teacher entity)
        {
            throw new NotImplementedException();
        }
    }
}
