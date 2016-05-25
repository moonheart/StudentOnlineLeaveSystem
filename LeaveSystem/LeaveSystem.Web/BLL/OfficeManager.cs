using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class OfficeManager : BaseManager<Office>, IOfficeManager
    {
        private IOfficeStore OfficeStore { get; set; }

        private bool _disposed;

        public OfficeManager(IOfficeStore baseStore)
            : base(baseStore)
        {
            OfficeStore = baseStore;
        }

        public override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
            base.Dispose();
        }

        public async Task<Office> FindOfficeByNameAsync(string name)
        {
            ThrowIfDisposed();
            return await OfficeStore.GetEntitiesByLamda(e => e.Name == name).SingleOrDefaultAsync();
        }

        public IQueryable<Office> GetOfficesForDepartment(Department department)
        {
            return OfficeStore.GetOfficesForDepartment(department);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
                this.OfficeStore.Dispose();
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        public static OfficeManager Create(IdentityFactoryOptions<OfficeManager> arg1, IOwinContext arg2)
        {
            return new OfficeManager(new OfficeStore(arg2.Get<AppDbContext>()));
        }
    }
}