using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LeaveSystem.Web.IDAL;

namespace LeaveSystem.Web.Models
{
    public class Department : Entity<int>
    {
        /// <summary>
        /// 院系名称
        /// </summary>
        [Required]
        [Display(Name = "学院名称")]
        public string Name { get; set; }

        public virtual ICollection<Major> Majors { get; set; }

        public virtual ICollection<Office> Offices { get; set; } 

    }
}