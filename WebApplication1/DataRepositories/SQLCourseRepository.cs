using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication1.Infrastructure;
using WebApplication1.Infrastructure.Repositories;
using WebApplication1.Models;

namespace WebApplication1.DataRepositories
{
    public class SQLCourseRepository : ICourseRepository
    {
        private readonly AppDbContext dbContext;
        public SQLCourseRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Course Delete(int id)
        {
            Course course = dbContext.Courses.Find(id);
            if (course != null) {
                dbContext.Courses.Remove(course);
                dbContext.SaveChanges();
            }
            return course;
        }

        public IQueryable<Course> GetAllCourses()
        {
            return dbContext.Courses;
        }

        public async Task<Course> GetCourseById(int id)
        {
            return await dbContext.Courses.FindAsync(id);
        }

        public Course Insert(Course course)
        {
            dbContext.Courses.Add(course);
            dbContext.SaveChanges();
            return course;
        }

        public async Task<int> InsertAsync(Course course)
        {
            await dbContext.Courses.AddAsync(course);
            return dbContext.SaveChanges();
        }

        public Course Update(Course course)
        {
            var updateCourse = dbContext.Courses.Attach(course);
            updateCourse.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            dbContext.SaveChanges();
            return course;
        }

        int IRepository<Course, int>.Count()
        {
            throw new NotImplementedException();
        }

        int IRepository<Course, int>.Count(Expression<Func<Course, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Task<int> IRepository<Course, int>.CountAsync()
        {
            throw new NotImplementedException();
        }

        Task<int> IRepository<Course, int>.CountAsync(Expression<Func<Course, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        void IRepository<Course, int>.Delete(Course entity)
        {
            throw new NotImplementedException();
        }

        void IRepository<Course, int>.Delete(Expression<Func<Course, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Task IRepository<Course, int>.DeleteAsync(Course entity)
        {
            throw new NotImplementedException();
        }

        Task IRepository<Course, int>.DeleteAsync(Expression<Func<Course, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Course IRepository<Course, int>.FirstOrDefault(Expression<Func<Course, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Task<Course> IRepository<Course, int>.FirstOrDefaultAsync(Expression<Func<Course, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        IQueryable<Course> IRepository<Course, int>.GetAll()
        {
            throw new NotImplementedException();
        }

        List<Course> IRepository<Course, int>.GetAllList()
        {
            throw new NotImplementedException();
        }

        List<Course> IRepository<Course, int>.GetAllList(Expression<Func<Course, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Task<List<Course>> IRepository<Course, int>.GetAllListAsync()
        {
            throw new NotImplementedException();
        }

        Task<List<Course>> IRepository<Course, int>.GetAllListAsync(Expression<Func<Course, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Task<Course> IRepository<Course, int>.InsertAsync(Course entity)
        {
            throw new NotImplementedException();
        }

        long IRepository<Course, int>.LongCount()
        {
            throw new NotImplementedException();
        }

        long IRepository<Course, int>.LongCount(Expression<Func<Course, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Task<long> IRepository<Course, int>.LongCountAsync()
        {
            throw new NotImplementedException();
        }

        Task<long> IRepository<Course, int>.LongCountAsync(Expression<Func<Course, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Course IRepository<Course, int>.Single(Expression<Func<Course, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Task<Course> IRepository<Course, int>.SingleAsync(Expression<Func<Course, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Task<Course> IRepository<Course, int>.UpdateAsync(Course entity)
        {
            throw new NotImplementedException();
        }
    }
}
