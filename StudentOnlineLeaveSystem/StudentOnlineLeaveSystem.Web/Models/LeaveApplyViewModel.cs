using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using StudentOnlineLeaveSystem.Models;

namespace StudentOnlineLeaveSystem.Web.Models
{
    /// <summary>
    /// 请假信息
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
        public string TimeType { get; set; }

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
        public string[] ImageUrls { get; set; }

    }
}