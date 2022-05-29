using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication1.Infrastructure.Repositories;
using WebApplication1.Models;

namespace WebApplication1.DataRepositories
{
    /// <summary>
    /// 课程数据操作接口
    /// </summary>
    public interface ICourseRepository:IRepository<Course,int>
    {
        Task<Course> GetCourseById(int id);
        IQueryable<Course> GetAllCourses();
        Course Delete(int id);
    }
}
