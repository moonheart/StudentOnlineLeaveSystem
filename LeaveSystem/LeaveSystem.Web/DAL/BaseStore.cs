using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.DAL
{
    /// <summary>
    /// 基础操作
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseStore<TEntity> : IBaseStore<TEntity> where TEntity : Entity<int>
    {

        protected EntityStore<TEntity> EntityStore;

        protected bool Disposed;

        protected DbContext Context { get; private set; }

        protected bool AutoSaveChanges { get; set; }
        protected bool DisposeContext { get; set; }

        public BaseStore(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            this.Context = context;
            this.EntityStore = new EntityStore<TEntity>(context);
            this.AutoSaveChanges = true;
        }

        protected void ThrowIfDisposed()
        {
            if (this.Disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);

        }

        public void Dispose(bool disposing)
        {
            if (this.DisposeContext && disposing && this.Context != null)
                this.Context.Dispose();
            this.Disposed = true;
            this.Context = (DbContext)null;
            this.EntityStore = (EntityStore<TEntity>)null;
        }

        protected async Task SaveChangesAsync()
        {
            if (this.AutoSaveChanges)
            {
                try
                {
                    int num = await this.Context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        protected void SaveChanges()
        {
            if (this.AutoSaveChanges)
            {
                int num = this.Context.SaveChanges();
            }
        }


        public virtual async Task<TEntity> CreateEntityAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            entity = EntityStore.Create(entity);
            await this.SaveChangesAsync();
            return entity;
        }

        public virtual TEntity CreateEntity(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            entity = EntityStore.Create(entity);
            this.SaveChanges();
            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> CreateEntitiesAsync(ICollection<TEntity> entities)
        {
            ThrowIfDisposed();
            if (entities == null)
                throw new ArgumentNullException("entities");
            var newEntities = entities.Select(entity => EntityStore.Create(entity));
            await this.SaveChangesAsync();
            return newEntities;
        }

        public virtual IEnumerable<TEntity> CreateEntities(ICollection<TEntity> entities)
        {
            ThrowIfDisposed();
            if (entities == null)
                throw new ArgumentNullException("entities");
            var newEntities = entities.Select(entity => EntityStore.Create(entity));
            this.SaveChanges();
            return newEntities;
        }

        public virtual async Task UpdateEntityAsync(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
                throw new ArgumentNullException("entity");
            EntityStore.Update(entity);
            await this.SaveChangesAsync();
        }

        public virtual void UpdateEntity(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
                throw new ArgumentNullException("entity");
            EntityStore.Update(entity);
            this.SaveChanges();
        }

        public virtual async Task UpdateEntitiesAsync(ICollection<TEntity> entities)
        {
            ThrowIfDisposed();
            if (entities == null)
                throw new ArgumentNullException("entities");
            foreach (var entity in entities)
            {
                EntityStore.Update(entity);
            }
            await this.SaveChangesAsync();
        }

        public virtual void UpdateEntities(ICollection<TEntity> entities)
        {
            ThrowIfDisposed();
            if (entities == null)
                throw new ArgumentNullException("entities");
            foreach (var entity in entities)
            {
                EntityStore.Update(entity);
            }
            this.SaveChanges();
        }

        public virtual async Task<TEntity> DeleteEntityAsync(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
                throw new ArgumentNullException("entity");
            var deleted = EntityStore.Delete(entity);
            await this.SaveChangesAsync();
            return deleted;
        }

        public virtual TEntity DeleteEntity(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
                throw new ArgumentNullException("entity");
            var deleted = EntityStore.Delete(entity);
            this.SaveChanges();
            return deleted;
        }

        public virtual async Task<IEnumerable<TEntity>> DeleteEntitiesAsync(ICollection<TEntity> entities)
        {
            ThrowIfDisposed();
            if (entities == null)
                throw new ArgumentNullException("entities");
            var list = entities.Select(e => EntityStore.Delete(e));
            await this.SaveChangesAsync();
            return list;
        }

        public virtual IEnumerable<TEntity> DeleteEntities(ICollection<TEntity> entities)
        {
            ThrowIfDisposed();
            if (entities == null)
                throw new ArgumentNullException("entities");
            var list = entities.Select(e => EntityStore.Delete(e));
            this.SaveChanges();
            return list;
        }

        public virtual async Task<TEntity> FindEntityByIdAsync(int id)
        {
            ThrowIfDisposed();
            var list = await EntityStore.EntitySet.SingleOrDefaultAsync(e => e.Id == id);
            return list;
        }

        public virtual TEntity FindEntityById(int id)
        {
            ThrowIfDisposed();
            var list = EntityStore.EntitySet.SingleOrDefault(e => e.Id == id);
            return list;
        }

        public virtual IQueryable<TEntity> GetAllEntities()
        {
            ThrowIfDisposed();
            return EntityStore.EntitySet;
        }

        public virtual IQueryable<TEntity> GetEntitiesByLamda(Expression<Func<TEntity, bool>> lamdaExpression)
        {
            ThrowIfDisposed();
            return EntityStore.EntitySet.Where(lamdaExpression);
        }

        public virtual async Task<bool> IsEntityExistByLamdaAsync(Expression<Func<TEntity, bool>> lamdaExpression)
        {
            ThrowIfDisposed();
            return await EntityStore.EntitySet.AnyAsync(lamdaExpression);
        }

        public virtual bool IsEntityExistByLamda(Expression<Func<TEntity, bool>> lamdaExpression)
        {
            ThrowIfDisposed();
            return EntityStore.EntitySet.Any(lamdaExpression);
        }
    }
}