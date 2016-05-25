using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IDAL
{
    public interface IMajorStore : IBaseStore<Major>
    {
        Task<Major> FindMajorByNameAsync(string name);

        IQueryable<Major> FindMajorsOfDepartment(Department department);

        IQueryable<Major> FindNoDepartmentMajors();

    }
}
