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
    public class GradeManager : BaseManager<Grade>, IGradeManager
    {
        private IGradeStore GradeStore { get; set; }

        private bool _disposed;

        public GradeManager(IGradeStore baseStore)
            : base(baseStore)
        {
            GradeStore = baseStore;
        }

        public override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
            base.Dispose();
        }

        public async Task<Grade> FindGradeByGradeNumberAsync(string num)
        {
            ThrowIfDisposed();
            return await GradeStore.FindGradeByNumberAsync(num);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
                this.GradeStore.Dispose();
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        public static GradeManager Create(IdentityFactoryOptions<GradeManager> arg1, IOwinContext arg2)
        {
            return new GradeManager(new GradeStore(arg2.Get<AppDbContext>()));
        }
    }
}