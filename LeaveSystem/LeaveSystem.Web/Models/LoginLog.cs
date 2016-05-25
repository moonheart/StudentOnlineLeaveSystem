using System;
using System.ComponentModel.DataAnnotations;
using LeaveSystem.Web.IDAL;

namespace LeaveSystem.Web.Models
{
    public class LoginLog : Entity<int>
    {
        /// <summary>
        /// 登录IP
        /// </summary>
        [MaxLength(20)]
        [Required]
        public string LoginIp { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        [Required]
        public DateTime LoginTime { get; set; }

        [Required]
        public string UserId { get; set; }

        public LoginLog(string loginIp, DateTime loginTime)
        {
            this.LoginIp = loginIp;
            this.LoginTime = loginTime;
        }

        public virtual User User { get; set; }
    }
}
