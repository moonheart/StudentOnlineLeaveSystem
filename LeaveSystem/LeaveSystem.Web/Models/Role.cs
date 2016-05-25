using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace LeaveSystem.Web.Models
{
    public class Role : IdentityRole<string, UserRole>
    {
        /// <summary>
        /// 角色说明
        /// </summary>
        [Required]
        [MinLength(2)]
        [DefaultValue("角色说明")]
        public string Description { get; set; }

        public string PermissionSequence { get; set; }

        public Role()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public Role(string roleName)
            : this()
        {
            this.Name = roleName;
        }


    }

}