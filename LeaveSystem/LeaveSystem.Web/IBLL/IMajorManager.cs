using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IBLL
{
    public interface IMajorManager : IBaseManager<Major>
    {
        Task<Major> FindMajorByNameAsync(string name);

        IQueryable<Major> GetMajorsForDepartment(Department de);

        IQueryable<Major> FindNoDepartmentMajors();

        Task SetMajorsDepartmentAsync(int[] majorIds, Department department);

        Task ResetMajorsDepartmentAsync(int[] majorIds);

    }
}
