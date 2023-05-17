using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseFramework.Infrastructure
{
    /// <summary>
    /// 定义仓储约定的接口
    /// </summary>
    /// <typeparam name="TEntity">传入仓储的实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">传入仓储的主键类型</typeparam>
    public interface IRepository<TEntity,TPrimaryKey> where TEntity : class
    {
        #region 查询

        IQueryable<TEntity> GetAll();

        List<TEntity> GetAllList();

        Task<List<TEntity>> GetAllListAsync();

        List<TEntity> GetAllList(Expression<Func<TEntity,bool>> predicate);

        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity,bool>> predicate);
        /// <summary>
        /// 通过条件查询实体信息，若没有找到，引发异常
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity Single(Expression<Func<TEntity,bool>> predicate);

        Task<TEntity> SingleAsync(Expression<Func<TEntity,bool>> predicate);
        /// <summary>
        /// 通过条件查询实体信息，若没有找到，返回null
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity FirstOrDefault(Expression<Func<TEntity,bool>> predicate);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity,bool>> predicate);



        #endregion


        #region  添加实体

        TEntity Insert(TEntity entity);

        Task<TEntity> InsertAsync(TEntity entity);


        #endregion

        #region 修改实体数据

        TEntity Update(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);


        #endregion

        #region 删除

        void Delete(TEntity entity);

        Task DeleteAsync(TEntity entity);

        void Delete(Expression<Func<TEntity,bool>> predicate);

        Task DeleteAsync(Expression<Func<TEntity,bool>> predicate);

        #endregion


        #region 计数方法

        int Count();

        Task<int> CountAsync();

        int Count(Expression<Func<TEntity,bool>> predicate);

        Task<int> CountAsync(Expression<Func<TEntity,bool>> predicate);

        long LongCount();

        Task<long> LongCountAsync();

        long LongCount(Expression<Func<TEntity,bool>> predicate);

        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

        #region  获取当前仓储内部的DbContext
        AppDbContext GetDbContext();
        #endregion

        #endregion
    }
}
