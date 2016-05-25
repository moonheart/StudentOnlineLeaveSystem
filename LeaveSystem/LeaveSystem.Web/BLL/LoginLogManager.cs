using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LeaveSystem.Web.DAL;
using LeaveSystem.Web.IDAL;
using LeaveSystem.Web.Infrastructure;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace LeaveSystem.Web.BLL
{
    //public class LoginLogManager : EntityManager<LoginLog, int>
    //{
    //    public LoginLogManager(IEntityStore<LoginLog, int> store)
    //        : base(store)
    //    {
    //    }

    //    public static LoginLogManager Create(IdentityFactoryOptions<LoginLogManager> options, IOwinContext context)
    //    {
    //        return new LoginLogManager(new EntityStore<LoginLog, int>(context.Get<AppDbContext>()));
    //    }

    //    public async Task<IdentityResult> WriteoginLogAsync(User user, DateTime loginTime, string loginIp)
    //    {
    //        var log = new LoginLog { LoginIp = loginIp, LoginTime = loginTime, User = user };
    //        return await this.CreateAsync(log);
    //    }
    //}
}