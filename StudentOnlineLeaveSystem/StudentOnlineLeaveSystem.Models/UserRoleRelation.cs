using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StudentOnlineLeaveSystem.Models
{
    /// <summary>
    /// 用户角色关系
    /// </summary>
    public class UserRoleRelation 
    {
        [Key]
        public int UserRoleRelationId { get; set; }

        ///// <summary>
        ///// 用户ID
        ///// </summary>
        //[Required()]
        //public int UserId { get; set; }

        ///// <summary>
        ///// 角色ID
        ///// </summary>
        //[Required()]
        //public int RoleId { get; set; }

        public virtual AppUser User { get; set; }

        public virtual Role Role { get; set; }

    }
}
