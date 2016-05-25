using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.DAL
{
    public class ResumeApplyStore : BaseStore<ResumeApply>, IResumeApplyStore
    {
        public ResumeApplyStore(DbContext context)
            : base(context)
        {
        }
    }
}