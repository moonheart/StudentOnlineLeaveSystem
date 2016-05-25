using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.ViewModels
{

    /// <summary>
    /// 角色更改
    /// </summary>
    public class RoleModificationViewModel
    {
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }

    /// <summary>
    /// 角色编辑
    /// </summary>
    public class RoleEditViewModel
    {
        public Role Role { get; set; }
        public IEnumerable<User> Members { get; set; }
        public IEnumerable<User> NonMembers { get; set; }
    }

    /// <summary>
    /// 角色创建
    /// </summary>
    public class RoleCreateViewModel
    {
        [Required]
        [Display(Name = "角色名称")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "描述")]
        public string Description { get; set; }

        public string[] UserIdsToAdd { get; set; }

        public ICollection<User> Users { get; set; } 

    }
}