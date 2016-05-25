using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentOnlineLeaveSystem.Models
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }

        [Display(Name = "权限名")]
        public string PermissionName { get; set; }

        [Display(Name = "权限值")]
        public int PermissionValue { get; set; }

        [Display(Name = "描述")]
        public string Descpription { get; set; }

        [Display(Name = "关联角色")]
        public virtual ICollection<RolePermissionRalation> RolePermissionRalations { get; set; } 
    }
}
