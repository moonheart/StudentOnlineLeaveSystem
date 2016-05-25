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
    /// 审核信息
    /// </summary>
    public class Check
    {
        [Key]
        public int CheckId { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        [Display(Name = "审核状态")]
        [Required]
        public CheckStatus CheckStatus { get; set; }

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
        [Required]
        public DateTime CheckTime { get; set; }

        /// <summary>
        /// 审核意见
        /// </summary>
        [Display(Name = "审核意见")]
        [DefaultValue("")]
        public string CheckOpinon { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        public virtual AppUser User { get; set; }

        /// <summary>
        /// 对应请假
        /// </summary>
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
