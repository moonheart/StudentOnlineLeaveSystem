using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentOnlineLeaveSystem.Models;

namespace StudentOnlineLeaveSystem.IBLL
{
    public interface IRoleService : IBaseService<Role>
    {
        /// <summary>
        /// 查找角色
        /// </summary>
        /// <param name="roleId">id</param>
        /// <returns></returns>
        Role Find(int roleId);


        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="pageIndex">页码数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalRecord">总记录数</param>
        /// <param name="order">排序：0-ID升序（默认），1ID降序，2注册时间升序，3注册时间降序，4登录时间升序，5登录时间降序</param>
        /// <returns></returns>
        IQueryable<Role> FindPageList(int pageIndex, int pageSize, out int totalRecord, int order);
    }
}
