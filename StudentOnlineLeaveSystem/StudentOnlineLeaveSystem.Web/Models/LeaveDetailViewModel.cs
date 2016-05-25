using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using StudentOnlineLeaveSystem.Models;

namespace StudentOnlineLeaveSystem.Web.Models
{
    public class LeaveDetailViewModel
    {
        public Leave Leave { get; set; }

        [Display(Name = "请假原因")]
        public string Reason { get { return Leave.Reason; } }

        [Display(Name = "类型")]
        public LeaveCategory Category { get { return Leave.Category; } }

        [Display(Name = "请假时间")]
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

        [Display(Name = "申请时间")]
        public string ApplyTime { get { return Leave.AddTime.ToString("yyyy-MM-dd HH:mm:ss"); } }

        [Display(Name = "审核信息")]
        public ICollection<Check> Checks { get; set; }

        [Display(Name = "是否销假")]
        public ResumeStatus IsResume { get { return Leave.IsResume; } }

        [Display(Name = "销假时间")]
        public DateTime ResumeTime { get { return Leave.ResumeTime; } }



    }
}