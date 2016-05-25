using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using LeaveSystem.Web.DAL;
using LeaveSystem.Web.IBLL;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace LeaveSystem.Web.BLL
{
    public class LeaveManager : BaseManager<Leave>, IDisposable, ILeaveManager
    {
        private bool _disposed;
        private LeaveStore LeaveStore { get; set; }
        public LeaveManager(LeaveStore store)
            : base(store)
        {
            LeaveStore = store;
        }

        public static LeaveManager Create(IdentityFactoryOptions<LeaveManager> options, IOwinContext context)
        {
            return new LeaveManager(new LeaveStore(context.Get<AppDbContext>()));
        }

        public IQueryable<Leave> GetLeavesOfUserAsync(User user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            return LeaveStore.FindLeavesByUserIdAsync(user.Id);
        }

        public IQueryable<Leave> GetUnResumeLeavesOfUserAsync(User user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            return LeaveStore.FindLeavesByUserIdAsync(user.Id)
                .Where(e => e.LeaveStatus != LeaveStatus.已取消)
                .Where(e => e.IsResume == ResumeStatus.未销假);
        }


        public async Task<bool> IsAllLeaveResumedAsync(string userId)
        {
            ThrowIfDisposed();
            var leaves = this.LeaveStore.GetAllEntities().Where(e => e.Student.Student.Id == userId);
            return await leaves.AllAsync(e => e.IsResume == ResumeStatus.已销假);
        }

        public async Task<bool> IsAllLeaveConditionMeeted(string userId, LeaveConfig config)
        {
            ThrowIfDisposed();
            if (config == null)
                throw new ArgumentNullException("config");
            var userLeaves = LeaveStore.GetAllEntities().Where(e => e.Student.Student.Id == userId);
            var time1 = DateTime.Now;
            var time2 = DateTime.Now.AddMonths(-1);
            var c1 = await userLeaves.Where(e => e.AddTime < time1 & e.AddTime > time2).ToListAsync();
            if (c1.Count() > config.MaxLeaveTimeInOneMonth)
            {
                return false;
            }
            return true;
        }

        public async Task<CheckStatus> GetLeaveStatus(int leaveId)
        {
            ThrowIfDisposed();
            var leave = await LeaveStore.FindEntityByIdAsync(leaveId);
            CheckStatus status = CheckStatus.未查看;
            if (leave != null)
            {
                foreach (Check check in leave.Checks)
                {
                    if (check.CheckStatus != CheckStatus.已通过)
                    {
                        status = check.CheckStatus;
                        break;
                    }
                    status = check.CheckStatus;
                }
            }
            return status;
        }

        public override void Dispose()
        {
            this.Dispose(true);
            base.Dispose();
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
                this.LeaveStore.Dispose();
            _disposed = true;
        }

        public void ResumeApply(int leaveId)
        {
            this.LeaveStore.ResumeApply(leaveId);
        }
    }
}