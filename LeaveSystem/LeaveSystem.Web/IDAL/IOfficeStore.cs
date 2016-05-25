using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IDAL
{
    public interface IOfficeStore : IDisposable, IBaseStore<Office>
    {
        IQueryable<Office> GetOfficesForDepartment(Department department);
    }
}
