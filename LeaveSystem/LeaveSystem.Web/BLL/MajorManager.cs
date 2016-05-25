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
    public class MajorManager : BaseManager<Major>, IMajorManager
    {
        private IMajorStore MajorStore { get; set; }

        private bool _disposed;

        public MajorManager(IMajorStore baseStore)
            : base(baseStore)
        {
            MajorStore = baseStore;
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
                this.MajorStore.Dispose();
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        public static MajorManager Create(IdentityFactoryOptions<MajorManager> arg1, IOwinContext arg2)
        {
            return new MajorManager(new MajorStore(arg2.Get<AppDbContext>()));
        }

        public async Task<Major> FindMajorByNameAsync(string name)
        {
            ThrowIfDisposed();
            return await MajorStore.FindMajorByNameAsync(name);
        }

        public IQueryable<Major> GetMajorsForDepartment(Department de)
        {
            ThrowIfDisposed();
            return MajorStore.FindMajorsOfDepartment(de);
        }
        public IQueryable<Major> FindNoDepartmentMajors()
        {
            ThrowIfDisposed();
            return MajorStore.FindNoDepartmentMajors();
        }

        public async Task SetMajorsDepartmentAsync(int[] majorIds, Department department)
        {
            ThrowIfDisposed();
            var list = await MajorStore.GetAllEntities().Where(e => majorIds.Contains(e.Id)).ToListAsync();
            foreach (var major in list)
            {
                major.Department = department;
            }
            await MajorStore.UpdateEntitiesAsync(list);
        }

        public async Task ResetMajorsDepartmentAsync(int[] majorIds)
        {
            ThrowIfDisposed();
            var list = await MajorStore.GetAllEntities().Where(e => majorIds.Contains(e.Id)).ToListAsync();
            foreach (var major in list)
            {
                major.Department = null;
            }
            await MajorStore.UpdateEntitiesAsync(list);
        }
    }
}