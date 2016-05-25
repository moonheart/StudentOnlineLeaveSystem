using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveSystem.Web.DAL;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using LeaveSystem.Web.Infrastructure;

namespace LeaveSystem.Web.BLL
{
    public class RoleManager : RoleManager<Role>
    {
        public RoleManager(RoleStore store)
            : base(store)
        {
        }

        public static RoleManager Create(IdentityFactoryOptions<RoleManager> options, IOwinContext context)
        {
            return new RoleManager(new RoleStore(context.Get<AppDbContext>()));
        }

        /// <summary>
        /// 获取非管理员角色
        /// </summary>
        /// <returns></returns>
        public IQueryable<Role> GetNonAdminRoles()
        {
            return Roles.Where(r => r.Name != "Administrator");
        }

        public IEnumerable<Role> GetRolesOfUser(string userId)
        {
            return Roles.Where(e => e.Users.Any(d => d.UserId == userId)).ToList();
        } 

    }
}