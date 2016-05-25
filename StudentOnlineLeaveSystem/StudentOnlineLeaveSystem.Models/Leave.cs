using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentOnlineLeaveSystem.Models
{
    /// <summary>
    /// 请假信息
    /// </summary>
    public class Leave
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Key]
        public int LeaveId { get; set; }

        #region 请假条信息

        /// <summary>
        /// 本人外出去向
        /// </summary>
        [Required]
        [Display(Name = "本人外出去向")]
        public string ToWhere { get; set; }

        /// <summary>
        /// 请假类型
        /// </summary>
        [Required]
        [Display(Name = "请假类型")]
        public LeaveCategory Category { get; set; }

        /// <summary>
        /// 请假原因
        /// </summary>
        [Required]
        [Display(Name = "请假原因")]
        public string Reason { get; set; }

        /// <summary>
        /// 时间类型
        /// </summary>
        [Required]
        [Display(Name = "时间类型")]
        public string TimeType { get; set; }

        /// <summary>
        /// 一天内的时间
        /// </summary>
        [Required]
        [Display(Name = "一天内的时间")]
        public DateTime OneDayTime { get; set; }

        /// <summary>
        /// 一天的开始节数
        /// </summary>
        [Required]
        [Display(Name = "一天的开始节数")]
        public int OneDayStart { get; set; }

        /// <summary>
        /// 一天的结束节数
        /// </summary>
        [Required]
        [Display(Name = "一天的结束节数")]
        public int OneDayEnd { get; set; }

        /// <summary>
        /// 请假开始日期
        /// </summary>
        [Required]
        [Display(Name = "请假开始日期")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 请假结束日期
        /// </summary>
        [Required]
        [Display(Name = "请假结束日期")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 图片附件地址
        /// </summary>
        [Display(Name = "图片附件地址")]
        public string[] ImageUrls { get; set; }

        [Required]
        [Display(Name = "用户ID")]
        public string UserId { get; set; }

        #endregion

        #region 数据信息

        /// <summary>
        /// 请假状态
        /// 0：未提交
        /// 1：已提交
        /// 2：已取消
        /// 9：已完成
        /// </summary>
        [Display(Name = "请假状态")]
        [DefaultValue(0)]
        [Required]
        public int LeaveStatus { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [Required]
        [Display(Name = "申请时间")]
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 是否销假
        /// </summary>
        [Required]
        [Display(Name = "是否销假")]
        public ResumeStatus IsResume { get; set; }

        /// <summary>
        /// 销假时间
        /// </summary>
        [Display(Name = "销假时间")]
        public DateTime ResumeTime { get; set; }

        #endregion

        #region 审核信息

        /// <summary>
        /// 审核信息
        /// </summary>
        public virtual ICollection<Check> Checks { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public virtual AppUser User { get; set; }

        #endregion

    }

    public enum LeaveCategory
    {
        病假 = 0,
        事假 = 1
    }

    public enum ResumeStatus
    {
        未销假 = 0,
        已销假 = 1
    }
}
