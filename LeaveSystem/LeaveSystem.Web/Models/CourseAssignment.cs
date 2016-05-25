using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveSystem.Web.Models
{
    public class CourseAssignment : Entity<int>
    {
        [Required]
        public virtual Course Course { get; set; }

        [Required]
        public int Week { get; set; }

        [Required]
        public int Day { get; set; }

        [Required]
        public int ClassNo { get; set; }

        [Required]
        public int Term { get; set; }

        [Required]
        public virtual User StudentUser { get; set; }

        [Required]
        public virtual User TeacherUser { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsLeave { get; set; }

        public virtual Leave LeaveInfo { get; set; }

    }
}