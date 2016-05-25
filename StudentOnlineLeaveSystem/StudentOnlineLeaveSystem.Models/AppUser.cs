using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StudentOnlineLeaveSystem.Models
{
    /// <summary>
    /// 用户模型
    /// </summary>
    public class AppUser : IdentityUser
    {
        //[Key]
        //public int UserId { get; set; }

        #region 用户基础信息

        //[Required(ErrorMessage = "必填")]
        //[StringLength(20, MinimumLength = 4, ErrorMessage = "{1}到{0}个字符")]
        //[Display(Name = "用户名")]
        //public string UserName { get; set; }

        /// <summary>
        /// 显示名
        /// </summary>
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{1}到{0}个字符")]
        [Display(Name = "显示名")]
        public string DisplayName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        ///// <summary>
        ///// 邮箱
        ///// </summary>
        //[Required(ErrorMessage = "必填")]
        //[Display(Name = "邮箱")]
        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }

        /// <summary>
        /// 用户状态<br />
        /// 0正常，1锁定，2未通过邮件验证，3未通过管理员
        /// </summary>
        [Display(Name = "状态")]
        public int Status { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }

        #endregion

        #region 权限信息

        /// <summary>
        /// 角色
        /// </summary>
        public virtual ICollection<UserRoleRelation> UserRoleRelations { get; set; }

        #endregion

        #region 业务信息

        /// <summary>
        /// 关联的审核信息
        /// </summary>
        public virtual ICollection<Check> Checks { get; set; }

        /// <summary>
        /// 关联的请假信息
        /// </summary>
        public virtual ICollection<Leave> Leaves { get; set; }

        /// <summary>
        /// 登陆记录
        /// </summary>
        public virtual ICollection<LoginLog> LoginLogs { get; set; }

        #endregion

    }
}
