using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LeaveSystem.Web.DAL;
using LeaveSystem.Web.IBLL;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace LeaveSystem.Web.BLL
{
    public class LeaveConfigManager : ILeaveConfigManager
    {
        private bool _disposed;

        private ILeaveConfigStore ConfigStore { get; set; }

        protected LeaveConfigManager(ILeaveConfigStore store)
        {
            if (store == null)
                throw new ArgumentNullException("store");
            ConfigStore = store;
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }
        public static LeaveConfigManager Create(IdentityFactoryOptions<LeaveConfigManager> options, IOwinContext context)
        {
            return new LeaveConfigManager(new LeaveConfigStore(context.Get<AppDbContext>()));
        }

        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
                this.ConfigStore.Dispose();
            _disposed = true;
        }

        public async Task<LeaveConfig> GetConfigAsync()
        {
            ThrowIfDisposed();
            return await this.ConfigStore.GetConfigAsync();
        }

        public LeaveConfig GetConfig()
        {
            ThrowIfDisposed();
            return this.ConfigStore.GetConfig();
        }

        public async Task SetConfigAsync(LeaveConfig config)
        {
            ThrowIfDisposed();
            if (config == null)
                throw new ArgumentNullException("config");
            await this.ConfigStore.SetConfigAsync(config);
        }

        public void SetConfig(LeaveConfig config)
        {
            ThrowIfDisposed();
            if (config == null)
                throw new ArgumentNullException("config");
            this.ConfigStore.SetConfig(config);
        }
    }
}