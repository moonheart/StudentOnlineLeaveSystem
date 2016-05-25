using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentOnlineLeaveSystem.IDAL;
using StudentOnlineLeaveSystem.Models;

namespace StudentOnlineLeaveSystem.DAL
{
    /// <summary>
    /// 请假信息仓库
    /// </summary>
    public class LeaveRepository:BaseRepository<Leave>,ILeaveRepository
    {
         
    }
}
