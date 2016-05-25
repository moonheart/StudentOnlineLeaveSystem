using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LeaveSystem.Web.Models
{
    public class TeacherInfo 
    {
        [Key,ForeignKey("Teacher")]
        public string TeacherId { get; set; }

        public virtual User Teacher { get; set; }

        public virtual Class ManageClass { get; set; }

        public virtual Position Position { get; set; }

        public virtual ICollection<Check> Checks { get; set; }

        public virtual ICollection<ResumeApply> ResumeApplies { get; set; }
        
        [Display(Name = "办公室")]
        public string OfficeNum { get; set; }

    }
}