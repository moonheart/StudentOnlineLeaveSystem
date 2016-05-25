using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.ViewModels
{
    public class PositionCreateViewModel
    {
        [Display(Name = "职位名称")]
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "负责人")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "职位描述")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "部门")]
        public int OfficeId { get; set; }

        [Display(Name = "学院")]
        public int DepartmentId { get; set; }


    }

    public class PositionEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "职位名称")]
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "负责人")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "职位描述")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "部门")]
        public int OfficeId { get; set; }

        [Display(Name = "学院")]
        public int DepartmentId { get; set; }

    }

}