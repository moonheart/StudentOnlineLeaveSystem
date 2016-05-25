using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentOnlineLeaveSystem.Models;

namespace StudentOnlineLeaveSystem.Models
{
    public class LoginLog
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public int LoginLogId { get; set; }
        /// <summary>
        /// 登录IP
        /// </summary>
        public string LoginIp { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }


        public AppUser User { get; set; }
    }
}
