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
    public class CheckManager : BaseManager<Check>, ICheckManager
    {
        private ICheckStore CheckStore { get; set; }
        private bool _disposed;

        #region conmon method
        public CheckManager(ICheckStore store)
            : base(store)
        {
            if (store == null)
                throw new ArgumentNullException("store");
            this.CheckStore = store;
        }

        public static CheckManager Create(IdentityFactoryOptions<CheckManager> options, IOwinContext context)
        {
            return new CheckManager(new CheckStore(context.Get<AppDbContext>()));
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }


        public override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
            base.Dispose();
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
                this.CheckStore.Dispose();
            _disposed = true;
        }

        #endregion

        public async Task<ICollection<Check>> GetTeachersCheckAsync(User user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            return await this.CheckStore.GetAllEntities()
                .Where(e => e.CheckTeacher.Teacher.Id == user.Id)
                .Where(e => e.CheckOrder == 0 
                    || e.Leave.Checks.Where(d => d.CheckOrder == 0)
                    .All(d => d.CheckStatus == CheckStatus.已通过 || d.CheckStatus == CheckStatus.未通过))
                .ToListAsync();
        }

        public ICollection<Check> GetTeachersCheck(User user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            return this.CheckStore.GetAllEntities().Where(e => e.CheckTeacher.Teacher.Id == user.Id).ToList();
        }

        public async Task<ICollection<Check>> GetTeachersNoCheckAsync(User user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            return await this.CheckStore.GetAllEntities()
                .Where(e => e.CheckTeacher.Teacher.Id == user.Id)
                .Where(e => e.CheckStatus == CheckStatus.已查看 || e.CheckStatus == CheckStatus.未查看)
                .ToListAsync();
        }

        public ICollection<Check> GetTeachersNoCheck(User user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            return CheckStore.GetAllEntities()
                .Where(e => e.CheckTeacher.Teacher.Id == user.Id)
                .Where(e => e.CheckStatus == CheckStatus.已查看 || e.CheckStatus == CheckStatus.未查看)
                .ToList();
        }

        public async Task<ICollection<Check>> GetToCheckAsync(User user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            //筛选本人
            var list = await this.CheckStore.GetAllEntities()
                .Where(e => e.Leave.LeaveStatus == LeaveStatus.已提交 || e.Leave.LeaveStatus == LeaveStatus.请假审核中)
                .Where(e => e.CheckTeacher.Teacher.Id == user.Id)
                .Where(e => e.CheckStatus == CheckStatus.已查看 || e.CheckStatus == CheckStatus.未查看).ToListAsync();
            //筛选顺序

            return
                list.Where(
                    check => check.CheckOrder == 0 || check.Leave.Checks.Where(e => e.CheckOrder == 0)
                        .All(e => e.CheckStatus == CheckStatus.未通过 || e.CheckStatus == CheckStatus.已通过))
                    .ToList();
        }
    }
}