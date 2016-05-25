using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace LeaveSystem.Web.ViewModels
{

    /// <summary>
    /// 用户登录
    /// </summary>
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "工号/学号")]
        [RegularExpression("^[0-9]{6}$|^[0-9]{12}$", ErrorMessage = "格式错误")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "验证码不正确")]
        [Display(Name = "验证码")]
        public string VerificationCode { get; set; }
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// 工号/学号
        /// </summary>
        [Required(ErrorMessage = "此项为必填")]
        [Display(Name = "工号/学号")]
        [RegularExpression("^[0-9]{6}$|^[0-9]{12}$", ErrorMessage = "格式错误")]
        public string UserName { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        [Required]
        [Display(Name = "姓名")]
        [StringLength(10, ErrorMessage = "{0} 必须包含 {1} 到 {2} 个字符。", MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "验证码不正确")]
        [Display(Name = "验证码")]
        public string VerificationCode { get; set; }
    }

    public class HeadSetViewModel
    {
        public string UserId { get; set; }

        public string UserHeadAddress { get; set; }

        public HttpPostedFileBase HeadFile { get; set; }
    }
}
