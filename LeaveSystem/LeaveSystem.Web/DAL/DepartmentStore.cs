using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.DAL
{
    public class DepartmentStore : BaseStore<Department>, IDepartmentStore
    {
        public DepartmentStore(DbContext context)
            : base(context)
        {
        }

        public async Task<Department> FindDepartmentByNameAsync(string departmentName)
        {
            ThrowIfDisposed();
            return await this.EntityStore.EntitySet.SingleOrDefaultAsync(d => d.Name == departmentName);
        }
    }
}