using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OA.IBLL
{
    public interface IBaseService<T> where T:class,new()
    {
        //查询方法
        IQueryable<T> GetEntities(Expression<Func<T, bool>> whereLambda);


        //分页查询方法
        IQueryable<T> GetPageEntities<S>(int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderByLambda, bool isAsc);

        //添加
        T Add(T entity);

        //修改
        bool Update(T entity);

        //删除
        bool Delete(T entity);
    }
}
