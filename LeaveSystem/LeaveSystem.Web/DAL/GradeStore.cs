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
    public class GradeStore : BaseStore<Grade>, IGradeStore
    {
        public GradeStore(DbContext context)
            : base(context)
        {
        }

        public Task<Grade> FindGradeByNumberAsync(string gradeNum)
        {
            return EntityStore.EntitySet.SingleOrDefaultAsync(e => e.GradeNum == gradeNum);
        }
    }
}