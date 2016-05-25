using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.DAL
{
    public class OfficeStore : BaseStore<Office>, IOfficeStore
    {
        public OfficeStore(DbContext context)
            : base(context)
        {
        }

        public IQueryable<Office> GetOfficesForDepartment(Department department)
        {
            ThrowIfDisposed();
            if (department == null)
                throw new ArgumentNullException("department");
            return EntityStore.EntitySet.Where(e => e.Department.Id == department.Id);

        }
    }
}