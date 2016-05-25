using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentOnlineLeaveSystem.Models
{
    public class RolePermissionRalation
    {
        [Key]
        public int RolePermissionRalationId { get; set; }

        [Display(Name = "权限ID")]
        public int PermissionId { get; set; }

        [Display(Name = "角色ID")]
        public int RoleId { get; set; }

        [Display(Name = "角色")]
        public virtual Role Role { get; set; }

        [Display(Name = "权限")]
        public virtual Permission Permission { get; set; }
    }
}
