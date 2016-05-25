using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveSystem.Web.BLL;
using LeaveSystem.Web.Models;
using Microsoft.AspNet.Identity;

namespace LeaveSystem.Web.Infrastructure
{
    public class CustomUserValidator : UserValidator<User>
    {
        public CustomUserValidator(UserManager mgr)
            : base(mgr)
        {
        }

        public override async Task<IdentityResult> ValidateAsync(User user)
        {
            IdentityResult result = await base.ValidateAsync(user);

            //if (!user.Email.ToLower().EndsWith("@gmail.com"))
            //{
            //    List<string> errors = result.Errors.ToList();
            //    errors.Add("Email 地址只支持gmail域名");
            //    result = new IdentityResult(errors);
            //}
            return result;
        }
    }
}
