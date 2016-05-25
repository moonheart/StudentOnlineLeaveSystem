using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LeaveSystem.Web.DAL;
using LeaveSystem.Web.IBLL;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace LeaveSystem.Web.BLL
{
    public class ClassManager : BaseManager<Class>, IClassManager
    {
        private IClassStore ClassStore { get; set; }

        private bool _disposed;

        public ClassManager(IClassStore baseStore)
            : base(baseStore)
        {
            ClassStore = baseStore;
        }

        public override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
            base.Dispose();
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
                this.ClassStore.Dispose();
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        public static ClassManager Create(IdentityFactoryOptions<ClassManager> arg1, IOwinContext arg2)
        {
            return new ClassManager(new ClassStore(arg2.Get<AppDbContext>()));
        }
    }
}