using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.ViewModels
{
    public class CheckIndexViewModel
    {
        public Check Check;

    }

    public class CheckApprovalViewModel
    {
        [Required]
        public int CheckId { get; set; }

        [Display(Name = "审核意见")]
        [StringLength(100, ErrorMessage = "不能超过{1}字"), MinLength(4)]
        [DefaultValue("审核意见")]
        [Required]
        public string CheckOpinion { get; set; }

        [Required]
        public int Option { get; set; }

        public CheckStatus Status { get; set; }
    }
}