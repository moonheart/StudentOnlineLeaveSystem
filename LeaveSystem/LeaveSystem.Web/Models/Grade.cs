using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveSystem.Web.Models
{
    public class Grade : Entity<int>
    {
        /// <summary>
        /// 年级号
        /// </summary>
        [Required]
        [Display(Name = "年级")]
        public string GradeNum { get; set; }
    }
}