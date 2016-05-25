using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using StudentOnlineLeaveSystem.DAL;
using StudentOnlineLeaveSystem.IBLL;
using StudentOnlineLeaveSystem.IDAL;
using StudentOnlineLeaveSystem.Models;

namespace StudentOnlineLeaveSystem.BLL
{
    /// <summary>
    /// 用户服务类
    /// </summary>
    public class UserService : BaseService<AppUser>, IUserService
    {
        public UserService()
            : base(RepositoryFactory.UserRepository)
        {
        }

        public bool Exist(string userName)
        {
            return CurrentRepository.Exist(u => u.UserName == userName);
        }

        public AppUser Find(string userId)
        {
            return CurrentRepository.Find(u => u.Id == userId);
        }

        public IQueryable<AppUser> FindPageList(int pageIndex, int pageSize, out int totalRecord, int order)
        {
            IQueryable<AppUser> users = CurrentRepository.Entities;
            switch (order)
            {
                case 0:
                    users = users.OrderBy(u => u.Id);
                    break;
                case 1:
                    users = users.OrderByDescending(u => u.Id);
                    break;
                default:
                    users = users.OrderBy(u => u.Id);
                    break;
            }
            totalRecord = users.Count();
            return PageList(users, pageIndex, pageSize);
        }

        public ClaimsIdentity CreateIdentity(AppUser user, string authenticationType)
        {
            ClaimsIdentity identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"));
            identity.AddClaim(new Claim("DisplayName", user.DisplayName));
            return identity;
        }
    }
}
