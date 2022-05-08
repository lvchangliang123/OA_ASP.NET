using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApplication1.Infrastructure.Repositories
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="TEntity">当前传入仓储的实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">传入仓储的主键类型</typeparam>
    public interface IRepository<TEntity,TPrimaryKey> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        List<TEntity> GetAllList();
        Task<List<TEntity>> GetAllListAsync();
        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity Single(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity Insert(TEntity entity);
        Task<TEntity> InsertAsync(TEntity entity);
        TEntity Update(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);

        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);
        void Delete(Expression<Func<TEntity, bool>> predicate);
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        int Count();
        Task<int> CountAsync();
        int Count(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        long LongCount();
        Task<long> LongCountAsync();
        long LongCount(Expression<Func<TEntity, bool>> predicate);
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);




    }
}
