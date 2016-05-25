using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentOnlineLeaveSystem.Models;

namespace StudentOnlineLeaveSystem.IBLL
{
    public interface ILeaveService : IBaseService<Leave>
    {
        /// <summary>
        /// 请假信息列表
        /// </summary>
        /// <param name="pageIndex">页码数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalRecord">总记录数</param>
        /// <param name="order">排序：0-ID升序（默认），1ID降序，2注册时间升序，3注册时间降序，4登录时间升序，5登录时间降序</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        IQueryable<Leave> FindPageList(int pageIndex, int pageSize, out int totalRecord, int order, string userId);

        /// <summary>
        /// 查找请假信息
        /// </summary>
        /// <param name="leaveId">请假ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        Leave Find(int leaveId,string userId);

        /// <summary>
        /// 获取请假次数
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        int GetLeaveTimes(string userId);

        /// <summary>
        /// 获取请假状态
        /// </summary>
        /// <param name="leaveId">请假ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        CheckStatus GetLeaveStatus(int leaveId, string userId);
    }
}
