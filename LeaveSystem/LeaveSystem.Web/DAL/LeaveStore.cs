using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.DAL
{
    public class LeaveStore :
        BaseStore<Leave>,
        ILeaveStore
    {
        private EntityStore<User> _userStore;
        public LeaveStore(DbContext context)
            : base(context)
        {
            _userStore = new EntityStore<User>(context);
        }

        public IQueryable<Leave> FindLeavesByUserIdAsync(string userId)
        {
            this.ThrowIfDisposed();
            return this.EntityStore.EntitySet.Where(e => e.Student.Student.Id == userId);
        }

        public void ResumeApply(int leaveId)
        {
            var leave = this.FindEntityById(leaveId);
            var teacher = leave.Checks.First(e => e.CheckOrder == 0).CheckTeacher;
            leave.ResumeApply = new ResumeApply()
            {
                AddTime = DateTime.Now,
                RecieveTeacher = teacher,
                ResumeApplyType = ResumeApplyType.未处理
            };
            this.UpdateEntity(leave);
            //teacher.TeacherInfo
            //_userStore.Update();
        }
    }
}