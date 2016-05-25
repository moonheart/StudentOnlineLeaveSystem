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
    public class DepartmentManager : BaseManager<Department>, IDepartmentManager
    {
        private IDepartmentStore DepartmentStore { get; set; }

        private bool _disposed;

        public DepartmentManager(IDepartmentStore baseStore)
            : base(baseStore)
        {
            DepartmentStore = baseStore;
        }

        public override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
            base.Dispose();
        }

        public async Task<Department> FindDepartmentByNameAsync(string name)
        {
            ThrowIfDisposed();
            return await this.DepartmentStore.FindDepartmentByNameAsync(name);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
                this.DepartmentStore.Dispose();
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        public static DepartmentManager Create(IdentityFactoryOptions<DepartmentManager> arg1, IOwinContext arg2)
        {
            return new DepartmentManager(new DepartmentStore(arg2.Get<AppDbContext>()));
        }

    }
}