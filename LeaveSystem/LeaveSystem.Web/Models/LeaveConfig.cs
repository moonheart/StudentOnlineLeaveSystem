using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LeaveSystem.Web.Models
{
    public class LeaveConfig
    {
        /// <summary>
        /// 非自增
        /// </summary>
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// 请病假2天以上需附病例复印件
        /// </summary>
        [Required]
        [Display(Name = "最短病假天数", Description = "请病假多少天以上需附病例复印件")]
        public int LeastSickLeaveDay { get; set; }

        /// <summary>
        /// 请假一天及以上者，假期结束后一天以前需到10507销假
        /// </summary>
        [Required]
        [Display(Name = "最短请销假天数", Description = "请假多少天及以上者，假期结束需销假")]
        public int LeastDayToSign { get; set; }

        /// <summary>
        /// 请假一天及以上者，假期结束后一天以前需到10507销假
        /// </summary>
        [Required]
        [Display(Name = "最短销假天数", Description = "假期结束后多少天以前需销假")]
        public int LeastResumeDay { get; set; }

        /// <summary>
        /// 请假一天及以上者，假期结束后一天以前需到10507销假
        /// </summary>
        [Display(Name = "销假办公室", Description = "销假办公室，如10507")]
        public string ResumeClassroom { get; set; }

        /// <summary>
        /// 销假教师
        /// </summary>
        [Display(Name = "销假教师")]
        public virtual TeacherInfo ResumeTeacher { get; set; }

        /// <summary>
        /// 销假教师
        /// </summary>
        [Display(Name = "销假教师")]
        [ForeignKey("ResumeTeacher")]
        public string ResumeTeacherId { get; set; }

        /// <summary>
        /// 一个月内请假次数
        /// </summary>
        [Required]
        [Display(Name = "最多请假次数(单月)", Description = "一个月内最多请假次数")]
        public int MaxLeaveTimeInOneMonth { get; set; }

        /// <summary>
        /// 一个月内请假天数
        /// </summary>
        [Required]
        [Display(Name = "最多请假天数(单月)", Description = "一个月内最多请假天数")]
        public int MaxLeaveDayInOneMonth { get; set; }

        /// <summary>
        /// 一月内请假节数
        /// </summary>
        [Required]
        [Display(Name = "最多请假节数(单月)", Description = "一个月内最多请假节数")]
        public int MaxLeaveClassInOneMonth { get; set; }

        /// <summary>
        /// 书记
        /// </summary>
        [Display(Name = "二审教师")]
        public virtual TeacherInfo ClerkTeacher { get; set; }

        /// <summary>
        /// 书记
        /// </summary>
        [Display(Name = "二审教师")]
        [ForeignKey("ClerkTeacher")]
        public string ClerkTeacherId { get; set; }

    }
}