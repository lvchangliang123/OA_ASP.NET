using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Infrastructure.Repositories
{
    public class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class
    {
        protected readonly AppDbContext dbContext;
        public virtual DbSet<TEntity> Table=>dbContext.Set<TEntity>();
        public RepositoryBase(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public int Count()
        {
            return GetAll().Count();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
           return GetAll().Where(predicate).Count();
        }

        public async Task<int> CountAsync()
        {
            return await GetAll().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).CountAsync();
        }

        public void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
            Save();
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetAll().Where(predicate).ToList()) {
                Delete(entity);
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
            await SaveAsync();
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetAll().Where(predicate).ToList()) {
                await DeleteAsync(entity);
            }
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate);
        }

        public IQueryable<TEntity> GetAll()
        {
            return Table.AsQueryable();
        }

        public List<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();  
        }

        public async Task<List<TEntity>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
           return await GetAll().Where(predicate).ToListAsync();
        }

        public TEntity Insert(TEntity entity)
        {
            var newEntity=Table.Add(entity).Entity;
            Save();
            return newEntity;
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            var newEntity = await Table.AddAsync(entity);
            await SaveAsync();
            return newEntity.Entity;
        }



        public long LongCount()
        {
            return GetAll().LongCount();
        }

        public async Task<long> LongCountAsync()
        {
            return await GetAll().LongCountAsync();
        }

        public long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).LongCount();
        }

        public async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).LongCountAsync();
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Single(predicate);
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
           return GetAll().SingleAsync(predicate);
        }

        public TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
            Save();
            return entity;
        }



        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            AttachIfNot(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
            await SaveAsync();
            return entity;
        }


        private void Save()
        {
          dbContext.SaveChanges();
        }

        private async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        private void AttachIfNot(TEntity entity)
        {
            var entry=dbContext.ChangeTracker.Entries().FirstOrDefault(ent=>ent.Entity==entity);   
            if (entry == null) {
                return;
            }
            Table.Attach(entity);
        }


    }
}
