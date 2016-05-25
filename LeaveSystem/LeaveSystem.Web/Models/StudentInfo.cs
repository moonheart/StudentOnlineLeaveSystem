using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveSystem.Web.Models
{
    public class StudentInfo
    {
        [Key, ForeignKey("Student")]
        public string StudentId { get; set; }

        public virtual User Student { get; set; }

        public virtual ICollection<Leave> Leaves { get; set; }

        public virtual Class BelongClass { get; set; }

        [Display(Name = "宿舍号")]
        public string Dormitory { get; set; }

        [Display(Name = "家庭住址")]
        public string HomeAddress { get; set; }

        [Display(Name = "家庭电话")]
        public string PersonalTel { get; set; }

    }
}