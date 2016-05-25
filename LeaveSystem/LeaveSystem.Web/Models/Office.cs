using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveSystem.Web.Models
{
    /// <summary>
    /// 部门
    /// </summary>
    public class Office : Entity<int>
    {
        [Required]
        [Display(Name = "部门")]
        public string Name { get; set; }

        [Required]
        [DefaultValue("请添加描述")]
        [Display(Name = "描述")]
        public string Description { get; set; }

        public virtual ICollection<Position> Positions { get; set; }

        [Required]
        public virtual Department Department { get; set; }
    }
}