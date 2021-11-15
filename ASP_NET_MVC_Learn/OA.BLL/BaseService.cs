using OA.DALFactory;
using OA.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OA.BLL
{
    public abstract class BaseService<T> where T:class,new()
    {

        public IBaseDal<T> CurrentDal { get; set; }
        public IDbSession DbSession
        {
            get
            {
                return DbSessionFactory.GetCurrentDbSession();
            }
        }

        public BaseService()
        {
            SetCurrentDal();
        }

        public abstract void SetCurrentDal();   //抽象方法在抽象类中，且子类必须实现该抽象方法 public修饰


        #region 查询方法
        //查询方法
        public IQueryable<T> GetEntities(Expression<Func<T, bool>> whereLambda)
        {
            return CurrentDal.GetEntities(whereLambda);
        }


        //分页查询方法
        public IQueryable<T> GetPageEntities<S>(int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderByLambda, bool isAsc)
        {
            return CurrentDal.GetPageEntities(pageSize, pageIndex, out total, whereLambda, orderByLambda, isAsc);
        }
        #endregion

        //添加
        public T Add(T entity)
        {
            CurrentDal.Add(entity);
            DbSession.SaveChanges();
            return entity;
        }

        //修改
        public bool Update(T entity)
        {
            CurrentDal.Update(entity);
            return DbSession.SaveChanges() > 0;
        }

        //删除
        public bool Delete(T entity)
        {
            CurrentDal.Delete(entity);
            return DbSession.SaveChanges() > 0;
        }
    }
}
