using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IDAL
{
    /// <summary>
    /// 学院操作
    /// </summary>
    public interface IDepartmentStore : IBaseStore<Department>
    {
        Task<Department> FindDepartmentByNameAsync(string departmentName);
    }
}
