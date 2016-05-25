using System;
using System.Linq;
using System.Threading.Tasks;
using LeaveSystem.Web.DAL;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace LeaveSystem.Web.BLL
{
    //public class EntityManager<TEntity, TKey> : IDisposable
    //    where TEntity : class, IEntity<TKey>, new()
    //    where TKey : IEquatable<TKey>
    //{

    //    private bool _disposed;
    //    protected internal IEntityStore<TEntity, TKey> Store { get; set; }


    //    public IQueryable<TEntity> Entities
    //    {
    //        get
    //        {
    //            IQueryableEntityStore<TEntity, TKey> queryableEntityStore = this.Store as IQueryableEntityStore<TEntity, TKey>;
    //            if (queryableEntityStore == null)
    //                throw new NotSupportedException(Resources.StoreNotIQueryableEntityStore);
    //            return queryableEntityStore.Entities;

    //        }
    //    }

    //    public EntityManager(IEntityStore<TEntity, TKey> store)
    //    {
    //        if (store == null)
    //            throw new ArgumentNullException("store");
    //        this.Store = store;
    //    }
    //    public void Dispose()
    //    {
    //        this.Dispose(true);
    //        GC.SuppressFinalize((object)this);
    //    }

    //    public virtual async Task<IdentityResult> CreateAsync(TEntity entity)
    //    {
    //        this.ThrowIfDisposed();
    //        if (entity == null)
    //            throw new ArgumentNullException("entity");
    //        await Infrastructure.TaskExtensions.WithCurrentCulture(this.Store.CreateAsync(entity));
    //        return IdentityResult.Success;
    //    }

    //    public virtual async Task<IdentityResult> UpdateAsync(TEntity entity)
    //    {
    //        this.ThrowIfDisposed();
    //        if ((object)entity == null)
    //            throw new ArgumentNullException("entity");
    //        await this.Store.UpdateAsync(entity);
    //        return IdentityResult.Success;
    //    }
    //    public virtual async Task<IdentityResult> DeleteAsync(TEntity entity)
    //    {
    //        this.ThrowIfDisposed();
    //        if ((object)entity == null)
    //            throw new ArgumentNullException("entity");
    //        await Store.DeleteAsync(entity);
    //        return IdentityResult.Success;
    //    }

    //    public virtual async Task<TEntity> FindByIdAsync(TKey key)
    //    {
    //        this.ThrowIfDisposed();
    //        return await Store.FindByIdAsync(key);
    //    }

    //    private void ThrowIfDisposed()
    //    {
    //        if (this._disposed)
    //            throw new ObjectDisposedException(this.GetType().Name);
    //    }
    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (!disposing || this._disposed)
    //            return;
    //        this.Store.Dispose();
    //        this._disposed = true;
    //    }


    //}

}