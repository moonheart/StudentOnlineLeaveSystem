using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.ViewModels
{
    /// <summary>
    /// 用户创建
    /// </summary>
    public class UserCreateViewModel
    {
        /// <summary>
        /// 工号/学号
        /// </summary>
        [Required(ErrorMessage = "此项为必填")]
        [Display(Name = "工号/学号")]
        [RegularExpression("^[0-9]{6}$|^[0-9]{12}$", ErrorMessage = "格式错误")]
        public string Number { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        [Required]
        [Display(Name = "姓名")]
        [StringLength(10, ErrorMessage = "{0} 必须包含 {1} 到 {2} 个字符。", MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        public string[] RolesToAdd { get; set; }
    }

    /// <summary>
    /// 用户编辑
    /// </summary>
    public class UserEditViewModel
    {
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// 工号/学号
        /// </summary>
        [Required(ErrorMessage = "此项为必填")]
        [Display(Name = "工号/学号")]
        [RegularExpression("^[0-9]{6}$|^[0-9]{12}$", ErrorMessage = "格式错误")]
        public string Number { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        [Required]
        [Display(Name = "姓名")]
        [StringLength(10, ErrorMessage = "{0} 必须包含 {1} 到 {2} 个字符。", MinimumLength = 2)]
        public string Name { get; set; }

        public string Password { get; set; }

        public string[] NamesToAdd { get; set; }

        public string[] NamesToRemove { get; set; }
    }

    ///// <summary>
    ///// 用户更改
    ///// </summary>
    //public class UserModifyViewModel
    //{
    //    [Required]
    //    public string Id { get; set; }
    //    [Required]
    //    public string UserName { get; set; }

    //    public string Password { get; set; }

    //    public string[] NamesToAdd { get; set; }

    //    public string[] NamesToRemove { get; set; }
    //}

}