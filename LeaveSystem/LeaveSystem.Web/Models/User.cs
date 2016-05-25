using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LeaveSystem.Web.Models
{
    public class User : IdentityUser<string, IdentityUserLogin, UserRole, IdentityUserClaim>, IUser
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }

        public User(string userName)
            : this()
        {
            UserName = userName;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            userIdentity.AddClaim(new Claim("Name", Name));
            return userIdentity;
        }

        [Display(Name = "学号/工号")]
        [RegularExpression("^[0-9]{6}$|^[0-9]{12}$", ErrorMessage = "格式错误")]
        public override string UserName { get; set; }

        public string HeadImage { get; set; }

        [Display(Name = "手机号")]
        public override string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        public virtual StudentInfo StudentInfo { get; set; }

        public virtual TeacherInfo TeacherInfo { get; set; }

        public virtual ICollection<LoginLog> LoginLogs { get; set; }
    }

}