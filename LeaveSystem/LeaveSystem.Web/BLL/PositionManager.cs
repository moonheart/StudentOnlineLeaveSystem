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
    public class PositionManager : BaseManager<Position>, IPositionManager
    {
        private IPositionStore PositionStore { get; set; }

        private bool _disposed;

        public PositionManager(IPositionStore baseStore)
            : base(baseStore)
        {
            PositionStore = baseStore;
        }

        public override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
            base.Dispose();
        }

        public async Task<Position> FindPositionByNameAsync(string name)
        {
            ThrowIfDisposed();
            return await PositionStore.FindPositionByNameAsync(name);
        }

        public IQueryable<Position> FindNoOfficePositions()
        {
            ThrowIfDisposed();
            return PositionStore.GetEntitiesByLamda(e => e.Office == null);
        }

        public async Task SetPositionsOfficeAsync(int[] ids, Office office)
        {
            ThrowIfDisposed();
            var list = await PositionStore.GetAllEntities().Where(d => ids.Contains(d.Id)).ToListAsync();
            foreach (var position in list)
            {
                position.Office = office;
            }
            await PositionStore.UpdateEntitiesAsync(list);
        }

        public async Task ResetPositionsOfficeAsync(int[] ids)
        {
            ThrowIfDisposed();
            var list = await PositionStore.GetAllEntities().Where(d => ids.Contains(d.Id)).ToListAsync();
            foreach (var position in list)
            {
                position.Office = null;
            }
            await PositionStore.UpdateEntitiesAsync(list);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
                this.PositionStore.Dispose();
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        public static PositionManager Create(IdentityFactoryOptions<PositionManager> arg1, IOwinContext arg2)
        {
            return new PositionManager(new PositionStore(arg2.Get<AppDbContext>()));
        }
    }
}