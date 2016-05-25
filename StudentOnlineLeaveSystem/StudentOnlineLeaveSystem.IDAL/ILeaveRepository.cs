using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentOnlineLeaveSystem.Models;

namespace StudentOnlineLeaveSystem.IDAL
{
    /// <summary>
    /// 请假信息接口
    /// </summary>
    public interface ILeaveRepository:IBaseRepository<Leave>
    {
    }
}
