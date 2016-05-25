using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LeaveSystem.Web.Models
{
    public class LessonAsign : Entity<int>
    {

        public int StartWeek { get; set; }

        public int EndWeek { get; set; }

        public int ClassSeq { get; set; }

        public WeekDay WeekDay { get; set; }

        public string Classroom { get; set; }

        [ForeignKey("Lesson")]
        public int LessonId { get; set; }

        public virtual LessonInfo Lesson { get; set; }

        [ForeignKey("Teacher")]
        public string TeacherId { get; set; }

        public virtual TeacherInfo Teacher { get; set; }

        [ForeignKey("Student")]
        public string StudentId { get; set; }

        public virtual StudentInfo Student { get; set; }
    }

    public enum WeekDay
    {
        周一,
        周二,
        周三,
        周四,
        周五,
        周六,
        周日
    }
}