using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentOnlineLeaveSystem.DAL;
using StudentOnlineLeaveSystem.IBLL;
using StudentOnlineLeaveSystem.IDAL;
using StudentOnlineLeaveSystem.Models;

namespace StudentOnlineLeaveSystem.BLL
{
    public class LeaveService : BaseService<Leave>, ILeaveService
    {
        public LeaveService()
            : base(RepositoryFactory.LeaveRepository)
        {
        }

        public IQueryable<Leave> FindPageList(int pageIndex, int pageSize, out int totalRecord, int order, string userId)
        {
            IQueryable<Leave> leaves = CurrentRepository.Entities.Where(l => l.User.Id == userId);
            switch (order)
            {
                case 0:
                    leaves = leaves.OrderBy(u => u.LeaveId);
                    break;
                default:
                    leaves = leaves.OrderBy(u => u.LeaveId);
                    break;
            }
            totalRecord = leaves.Count();

            return PageList(leaves, pageIndex, pageSize);

        }

        public Leave Find(int leaveId, string userId)
        {
            var leave = CurrentRepository.Entities
                .Where(l => l.User.Id == userId)
                .Single(l => l.LeaveId == leaveId);
            return leave;
        }

        public int GetLeaveTimes(string userId)
        {
            return CurrentRepository.Entities.Count(l => l.UserId == userId);
        }

        public CheckStatus GetLeaveStatus(int leaveId, string userId)
        {
            var leave = CurrentRepository.Entities.Where(l => l.UserId == userId).Single(l => l.LeaveId == leaveId);
            CheckStatus status = CheckStatus.未查看;
            foreach (Check check in leave.Checks)
            {
                if (check.CheckStatus != CheckStatus.已通过)
                {
                    status = check.CheckStatus;
                    break;
                }
                status = check.CheckStatus;
            }
            return status;
        }
    }
}
