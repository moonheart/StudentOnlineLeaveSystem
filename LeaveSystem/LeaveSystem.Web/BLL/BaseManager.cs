using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using LeaveSystem.Web.DAL;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Threading.Tasks;
using LeaveSystem.Web.IBLL;

namespace LeaveSystem.Web.BLL
{
    public abstract class BaseManager<TEntity> : IDisposable, IBaseManager<TEntity> where TEntity : Entity<int>
    {
        private bool _disposed;

        private IBaseStore<TEntity> BaseStore { get; set; }

        protected BaseManager(IBaseStore<TEntity> baseStore)
        {
            if (baseStore == null)
                throw new ArgumentNullException("baseStore");
            BaseStore = baseStore;
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
                this.BaseStore.Dispose();
            _disposed = true;
        }

        public virtual async Task<TEntity> AddEntityAsync(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
                throw new ArgumentNullException("entity");
            return await BaseStore.CreateEntityAsync(entity);
        }

        public virtual TEntity AddEntity(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
                throw new ArgumentNullException("entity");
            return BaseStore.CreateEntity(entity);
        }

        public virtual async Task<IEnumerable<TEntity>> AddEntitiesAsync(ICollection<TEntity> entities)
        {
            ThrowIfDisposed();
            if (entities == null)
                throw new ArgumentNullException("entities");
            return await BaseStore.CreateEntitiesAsync(entities);
        }

        public virtual IEnumerable<TEntity> AddEntities(ICollection<TEntity> entities)
        {
            ThrowIfDisposed();
            if (entities == null)
                throw new ArgumentNullException("entities");
            return BaseStore.CreateEntities(entities);
        }

        public virtual async Task UpdateEntityAsync(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
                throw new ArgumentNullException("entity");
            await BaseStore.UpdateEntityAsync(entity);
        }

        public virtual void UpdateEntity(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
                throw new ArgumentNullException("entity");
            BaseStore.UpdateEntity(entity);
        }

        public virtual async Task UpdateEntitiesAsync(ICollection<TEntity> entities)
        {
            ThrowIfDisposed();
            if (entities == null)
                throw new ArgumentNullException("entities");
            await BaseStore.UpdateEntitiesAsync(entities);
        }

        public virtual void UpdateEntities(ICollection<TEntity> entities)
        {
            ThrowIfDisposed();
            if (entities == null)
                throw new ArgumentNullException("entities");
            BaseStore.UpdateEntities(entities);
        }

        public virtual async Task DeleteEntityAsync(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
                throw new ArgumentNullException("entity");
            await BaseStore.DeleteEntityAsync(entity);
        }

        public virtual void DeleteEntity(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
                throw new ArgumentNullException("entity");
            BaseStore.DeleteEntity(entity);
        }

        public virtual async Task DeleteEntitiesAsync(ICollection<TEntity> entities)
        {
            ThrowIfDisposed();
            if (entities == null)
                throw new ArgumentNullException("entities");
            await BaseStore.DeleteEntitiesAsync(entities);
        }

        public virtual void DeleteEntities(ICollection<TEntity> entities)
        {
            ThrowIfDisposed();
            if (entities == null)
                throw new ArgumentNullException("entities");
            BaseStore.DeleteEntities(entities);
        }

        public virtual async Task<TEntity> FindEntityByIdAsync(int id)
        {
            ThrowIfDisposed();
            return await BaseStore.FindEntityByIdAsync(id);
        }

        public virtual TEntity FindEntityById(int id)
        {
            ThrowIfDisposed();
            return BaseStore.FindEntityById(id);
        }

        public virtual IQueryable<TEntity> GetAllEntities()
        {
            ThrowIfDisposed();
            return BaseStore.GetAllEntities();
        }

        public virtual IQueryable<TEntity> GetEntitiesByLamda(Expression<Func<TEntity, bool>> lamdaExpression)
        {
            ThrowIfDisposed();
            return BaseStore.GetEntitiesByLamda(lamdaExpression);
        }

        public virtual async Task<bool> IsEntityExistByLamdaAsync(Expression<Func<TEntity, bool>> lamdaExpression)
        {
            ThrowIfDisposed();
            return await BaseStore.IsEntityExistByLamdaAsync(lamdaExpression);
        }

        public virtual bool IsEntityExistByLamda(Expression<Func<TEntity, bool>> lamdaExpression)
        {
            ThrowIfDisposed();
            return BaseStore.IsEntityExistByLamda(lamdaExpression);
        }
    }
}