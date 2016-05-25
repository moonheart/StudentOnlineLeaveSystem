using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LeaveSystem.Web.IDAL;

namespace LeaveSystem.Web.Models
{
    /// <summary>
    /// 审核信息
    /// </summary>
    public class Check : Entity<int>
    {
        /// <summary>
        /// 审核状态
        /// </summary>
        [Display(Name = "审核状态")]
        [Required]
        public virtual CheckStatus CheckStatus { get; set; }

        /// <summary>
        /// 审核顺序
        /// </summary>
        [Display(Name = "审核顺序")]
        [Required]
        public int CheckOrder { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [Display(Name = "审核时间")]
        public DateTime CheckTime { get; set; }

        [Display(Name = "添加时间")]
        [Required]
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 审核意见
        /// </summary>
        [Display(Name = "审核意见")]
        [StringLength(100, ErrorMessage = "不能超过{1}字"), MinLength(4)]
        [DefaultValue("审核意见")]
        public string CheckOpinion { get; set; }

        [Required]
        public virtual TeacherInfo CheckTeacher { get; set; }

        /// <summary>
        /// 对应请假
        /// </summary>
        [Required]
        public virtual Leave Leave { get; set; }

    }

    public enum CheckStatus
    {
        未查看 = 0,
        已查看 = 1,
        未通过 = 2,
        已通过 = 3
    }
}