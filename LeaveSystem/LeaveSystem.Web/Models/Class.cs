using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveSystem.Web.Models
{
    public class Class : Entity<int>
    {
        [Required]
        public virtual Major Major { get; set; }

        [Required]
        public virtual Grade Grade { get; set; }

        [Required]
        [Display(Name = "班别")]
        public string ClassDefination { get; set; }

        //public virtual ICollection<User> Students { get; set; }

        [Display(Name = "班主任")]
        [Required]
        public virtual TeacherInfo ClassTeacher { get; set; }
    }
}