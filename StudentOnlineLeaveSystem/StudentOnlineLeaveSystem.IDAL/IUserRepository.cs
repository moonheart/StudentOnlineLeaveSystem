using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentOnlineLeaveSystem.Models;

namespace StudentOnlineLeaveSystem.IDAL
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public interface IUserRepository : IBaseRepository<AppUser>
    {
    }
}
