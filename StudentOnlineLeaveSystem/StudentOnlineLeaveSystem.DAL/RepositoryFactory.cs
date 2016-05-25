using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentOnlineLeaveSystem.IDAL;

namespace StudentOnlineLeaveSystem.DAL
{
    /// <summary>
    /// 简单工厂
    /// </summary>
    public static class RepositoryFactory
    {
        /// <summary>
        /// 用户仓库
        /// </summary>
        public static IUserRepository UserRepository { get { return new UserRepository(); } }

        /// <summary>
        /// 请假信息仓库
        /// </summary>
        public static ILeaveRepository LeaveRepository { get { return new LeaveRepository(); } }

        /// <summary>
        /// 角色信息仓库
        /// </summary>
        public static IRoleRepository RoleRepository { get { return new RoleRepository(); } }
    }
}
