using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LeaveSystem.Web.DAL;
using LeaveSystem.Web.IBLL;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace LeaveSystem.Web.BLL
{
    public class ResumeManager : BaseManager<ResumeApply>, IResumeManager
    {
        public ResumeManager(IBaseStore<ResumeApply> baseStore)
            : base(baseStore)
        {
        }

        public static ResumeManager Create(IdentityFactoryOptions<ResumeManager> arg1, IOwinContext arg2)
        {
            return new ResumeManager(new ResumeApplyStore(arg2.Get<AppDbContext>()));
        }
    }
}