using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.DAL
{
    public class LeaveConfigStore : ILeaveConfigStore
    {
        protected EntityStore<LeaveConfig> ConfigStore;

        protected bool Disposed;

        protected DbContext Context { get; private set; }

        protected bool AutoSaveChanges { get; set; }
        protected bool DisposeContext { get; set; }


        public LeaveConfigStore(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            this.Context = context;
            this.ConfigStore = new EntityStore<LeaveConfig>(context);
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
            this.ConfigStore = (EntityStore<LeaveConfig>)null;
        }

        protected async Task SaveChangesAsync()
        {
            if (this.AutoSaveChanges)
            {
                int num = await this.Context.SaveChangesAsync();
            }
        }

        protected void SaveChanges()
        {
            if (this.AutoSaveChanges)
            {
                int num = this.Context.SaveChanges();
            }
        }

        public async Task<LeaveConfig> GetConfigAsync()
        {
            ThrowIfDisposed();
            return await this.ConfigStore.EntitySet.SingleOrDefaultAsync(e => e.Id == 1);
        }

        public LeaveConfig GetConfig()
        {
            ThrowIfDisposed();
            return this.ConfigStore.EntitySet.SingleOrDefault(e => e.Id == 1);
        }

        public async Task SetConfigAsync(LeaveConfig config)
        {
            ThrowIfDisposed();
            this.ConfigStore.Update(config);
            await this.SaveChangesAsync();
        }

        public void SetConfig(LeaveConfig config)
        {
            ThrowIfDisposed();
            this.ConfigStore.Update(config);
            this.SaveChanges();
        }
    }
}