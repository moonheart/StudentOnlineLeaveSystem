using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveSystem.Web.Models
{
    public class LessonInfo : Entity<int>
    {
        public string LessonName { get; set; }

        public virtual Department BelongDepartment { get; set; }
    }
}