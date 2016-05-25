using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.ViewModels
{
    /// <summary>
    /// 请假申请
    /// </summary>
    public class LeaveApplyViewModel
    {
        /// <summary>
        /// 本人外出去向
        /// </summary>
        [Display(Name = "本人外出去向")]
        [Required(ErrorMessage = "请填写{0}")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "{1}到{0}个字符")]
        public string ToWhere { get; set; }

        /// <summary>
        /// 请假类型
        /// </summary>
        [Display(Name = "请假类型")]
        [Required]
        [DefaultValue("0")]
        public LeaveCategory Category { get; set; }

        /// <summary>
        /// 请假原因
        /// </summary>
        [Display(Name = "请假原因")]
        [Required(ErrorMessage = "请填写{0}")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "{1}到{0}个字符")]
        public string Reason { get; set; }

        /// <summary>
        /// 时间类型
        /// </summary>
        [Display(Name = "时间类型")]
        [Required]
        [DefaultValue("0")]
        public TimeType TimeType { get; set; }

        /// <summary>
        /// 一天内的时间
        /// </summary>
        [Display(Name = "一天内的时间")]
        [Required]
        [DefaultValue(typeof(DateTime), "1990-01-01")]
        public DateTime OneDayTime { get; set; }

        /// <summary>
        /// 一天的开始节数
        /// </summary>
        [Display(Name = "一天的开始节数")]
        [Required]
        [DefaultValue(0)]
        public int OneDayStart { get; set; }

        /// <summary>
        /// 一天的结束节数
        /// </summary>
        [Display(Name = "一天的结束节数")]
        [Required]
        [DefaultValue(0)]
        public int OneDayEnd { get; set; }

        /// <summary>
        /// 请假开始日期
        /// </summary>
        [Display(Name = "请假开始日期")]
        [Required]
        [DefaultValue(typeof(DateTime), "1990-01-01")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 请假结束日期
        /// </summary>
        [Display(Name = "请假结束日期")]
        [Required]
        [DefaultValue(typeof(DateTime), "1990-01-01")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 图片附件地址
        /// </summary>
        [Display(Name = "图片附件地址")]
        public string ImageUrls { get; set; }

        public HttpPostedFileBase UploadFile { get; set; }

    }

    /// <summary>
    /// 请假详情
    /// </summary>
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
                    case TimeType.一天内:
                        res = string.Format("{0} 的第 {1} 节课到第 {2} 节课", Leave.OneDayTime.ToString("yyyy年M月d日"),
                            Leave.OneDayStart, Leave.OneDayEnd);
                        break;
                    case TimeType.一天以上:
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

    /// <summary>
    /// 请假列表
    /// </summary>
    public class LeaveListViewModel
    {
        public Leave Leave;

        public int LeaveId
        {
            get { return Leave.Id; }
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
                    case TimeType.一天内:
                        res = string.Format("{0} 的第 {1} 节课到第 {2} 节课", Leave.OneDayTime.ToString("yyyy年M月d日"),
                            Leave.OneDayStart, Leave.OneDayEnd);
                        break;
                    case TimeType.一天以上:
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

    public class AddLeaveImagesViewModel
    {
        [Required]
        public string ImagesUrl { get; set; }
    }
}