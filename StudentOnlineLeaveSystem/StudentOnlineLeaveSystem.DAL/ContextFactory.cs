using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace StudentOnlineLeaveSystem.DAL
{
    /// <summary>
    /// 简单工厂
    /// </summary>
    public class ContextFactory
    {
        /// <summary>
        /// 获取当前数据库上下文
        /// </summary>
        /// <returns></returns>
        public static StudentOnlineLeaveSystemDbContext GetCurrentContext()
        {
            StudentOnlineLeaveSystemDbContext context = CallContext.GetData("StudentOnlineLeaveSystemDbContext") as StudentOnlineLeaveSystemDbContext;
            if (context == null)
            {
                context = new StudentOnlineLeaveSystemDbContext();
                CallContext.SetData("StudentOnlineLeaveSystemDbContext", context);
            }
            return context;
        }
    }
}
