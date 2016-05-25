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
    /// 用户组
    /// </summary>
    public class Role : IdentityRole
    {
        [Key]
        public int RoleId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{1}到{0}个字")]
        [Display(Name = "名称")]
        public string RoleName { get; set; }

        /// <summary>
        /// 用户组类型<br />
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [Display(Name = "用户组类型")]
        public int RoleValue { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(50, ErrorMessage = "少于{0}个字")]
        [Display(Name = "说明")]
        public string Description { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [Display(Name = "用户角色")]
        public virtual ICollection<UserRoleRelation> UserRoleRelations { get; set; }

        [Display(Name = "角色权限")]
        public virtual ICollection<RolePermissionRalation> RolePermissionRalations { get; set; }

    }
}
