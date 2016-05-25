using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity;
using TaskExtensions = LeaveSystem.Web.Infrastructure.TaskExtensions;

namespace LeaveSystem.Web.DAL
{
    public class EntityStore<TEntity> where TEntity : class
    {
        public DbContext Context { get; private set; }

        public IQueryable<TEntity> EntitySet
        {
            get
            {
                return (IQueryable<TEntity>)this.DbEntitySet;
            }
        }

        public DbSet<TEntity> DbEntitySet { get; private set; }

        public EntityStore(DbContext context)
        {
            this.Context = context;
            this.DbEntitySet = context.Set<TEntity>();
        }

        public virtual Task<TEntity> GetByIdAsync(object id)
        {
            return this.DbEntitySet.FindAsync(id);
        }

        public TEntity Create(TEntity entity)
        {
            return this.DbEntitySet.Add(entity);
        }

        public TEntity Delete(TEntity entity)
        {
            return this.DbEntitySet.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            if ((object)entity == null)
                return;
            this.Context.Entry<TEntity>(entity).State = EntityState.Modified;
        }
    }
}
