using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveSystem.Web.Models
{
    /// <summary>
    /// 职位
    /// </summary>
    public class Position : Entity<int>
    {
        [Display(Name = "职位名称")]
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "负责人")]
        public virtual TeacherInfo HeadUser { get; set; }

        [Required]
        [Display(Name = "职位描述")]
        public string Description { get; set; }

        [Required]
        public virtual Office Office { get; set; }
    }
}