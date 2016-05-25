using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IBLL
{
    public interface IOfficeManager : IBaseManager<Office>
    {
        Task<Office> FindOfficeByNameAsync(string name);

        IQueryable<Office> GetOfficesForDepartment(Department department);
    }
}
