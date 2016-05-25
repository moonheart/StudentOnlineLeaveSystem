using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using StudentOnlineLeaveSystem.Models;

namespace StudentOnlineLeaveSystem.Web.Models
{
    public class LeaveListViewModel
    {
        public Leave Leave;

        public int LeaveId
        {
            get { return Leave.LeaveId; }
        }

        [Display(Name = "类型")]
        public LeaveCategory LeaveType { get { return Leave.Category; } }

        [Display(Name = "原因")]
        public string LeaveReason
        {
            get { return Leave.Reason; }
        }

        [Display(Name = "去向")]
        public string ToWhere
        {
            get { return Leave.ToWhere; }
        }

        [Display(Name = "时间")]
        public string LeaveTime
        {
            get
            {
                string res = "";
                switch (Leave.TimeType)
                {
                    case "0":
                        res = string.Format("{0} 的第 {1} 节课到第 {2} 节课", Leave.OneDayTime.ToString("yyyy年M月d日"),
                            Leave.OneDayStart, Leave.OneDayEnd);
                        break;
                    case "1":
                        res = string.Format("{0} 到 {1}", Leave.StartDate.ToString("yyyy年M月d日"),
                            Leave.EndDate.ToString("yyyy年M月d日"));
                        break;
                }
                return res;
            }
        }

        [Display(Name = "状态")]
        public CheckStatus Status { get; set; }
    }
}