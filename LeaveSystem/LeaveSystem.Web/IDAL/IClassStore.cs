using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IDAL
{
    /// <summary>
    /// 班级存储接口
    /// </summary>
    public interface IClassStore : IBaseStore<Class>
    {
        Task<bool> IsClassExistAsync(Expression<Func<Class, bool>> lamda);
    }
}
