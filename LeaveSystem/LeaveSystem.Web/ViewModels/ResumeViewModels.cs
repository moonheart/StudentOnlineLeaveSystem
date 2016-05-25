using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.ViewModels
{
    public class ResumeProcessViewModel
    {
        public int LeaveId { get; set; }

        public ResumeApplyType ResumeApplyType { get; set; }
    }
}