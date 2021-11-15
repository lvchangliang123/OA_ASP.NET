using OA.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OA.EFDAL
{
    /// <summary>
    /// 封装CRUD操作方法的基类
    /// T:传入泛型，是一个类，执行默认构造方法
    /// </summary>
    public class BaseDal<T> where T:class,new()
    {
        //oaEntities db = new oaEntities();
        //DbContext是oaEntities的基类，下面的方法调用的都是基类的方法
        public DbContext Db
        {
            get { return DbContextFactory.GetCurrentDbContext(); }
        }

        //查询方法
        public IQueryable<T> GetEntities(Expression<Func<T,bool>> whereLambda)
        {
            return Db.Set<T>().Where(whereLambda).AsQueryable();
        }


        //分页查询方法
        public IQueryable<T> GetPageEntities<S>(int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderByLambda, bool isAsc)
        {
            total = Db.Set<T>().Where(whereLambda).Count();
            IQueryable<T> temp = null;
            if (isAsc)
            {
                temp = Db.Set<T>().Where(whereLambda).OrderBy<T, S>(orderByLambda).Skip(pageSize * (pageIndex - 1)).Take(pageSize).AsQueryable();
            }
            else
            {
                temp = Db.Set<T>().Where(whereLambda).OrderByDescending<T, S>(orderByLambda).Skip(pageSize * (pageIndex - 1)).Take(pageSize).AsQueryable();
            }

            return temp;
        }

        //添加
        public T Add(T entity)
        {
            Db.Set<T>().Add(entity);
            //Db.SaveChanges();
            return entity;
        }

        //修改
        public bool Update(T entity)
        {
            Db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            //return Db.SaveChanges() > 0;
            return true;
        }

        //删除
        public bool Delete(T entity)
        {
            Db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            //return Db.SaveChanges() > 0;
            return true;
        }


    }
}
