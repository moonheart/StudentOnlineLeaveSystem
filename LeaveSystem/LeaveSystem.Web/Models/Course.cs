using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveSystem.Web.Models
{
    public class Course : Entity<int>
    {
        [Required]
        [Display(Name = "课程名")]
        public string Name { get; set; }


    }
}