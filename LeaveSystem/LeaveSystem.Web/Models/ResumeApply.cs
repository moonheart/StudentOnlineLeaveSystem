using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveSystem.Web.Models
{
    public class ResumeApply : Entity<int>
    {
        [Required]
        public virtual Leave Leave { get; set; }

        [Required]
        public virtual TeacherInfo RecieveTeacher { get; set; }

        [Required]
        public ResumeApplyType ResumeApplyType { get; set; }

        public DateTime AddTime { get; set; }

        public DateTime? ProcessTime { get; set; }
    }

    public enum ResumeApplyType
    {
        未处理 = 0,
        通过 = 1,
    }
}