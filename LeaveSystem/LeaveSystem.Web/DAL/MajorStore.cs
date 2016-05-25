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
    public class MajorStore : BaseStore<Major>, IMajorStore
    {
        public MajorStore(DbContext context)
            : base(context)
        {
        }

        public async Task<Major> FindMajorByNameAsync(string name)
        {
            ThrowIfDisposed();
            return await EntityStore.EntitySet.SingleOrDefaultAsync(m => m.Name == name);
        }

        public IQueryable<Major> FindMajorsOfDepartment(Department department)
        {
            ThrowIfDisposed();
            if (department == null)
                throw new ArgumentNullException("department");
            return EntityStore.EntitySet.Where(e => e.Department.Id == department.Id);
        }

        public IQueryable<Major> FindNoDepartmentMajors()
        {
            ThrowIfDisposed();
            return EntityStore.EntitySet.Where(e => e.Department == null);
        }
    }
}