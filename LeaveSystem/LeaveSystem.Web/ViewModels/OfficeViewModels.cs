using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.ViewModels
{
    public class OfficeCreateViewModel
    {
        [Required]
        [Display(Name = "部门名称")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "描述")]
        public string Description { get; set; }

        public int[] IdsToAdd { get; set; }

        [Required]
        [Display(Name = "隶属学院")]
        public int DepartmentId { get; set; }

    }

    public class OfficeEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "部门名称")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "描述")]
        public string Description { get; set; }

        public int[] IdsToRemove { get; set; }

        [Required]
        [Display(Name = "隶属学院")]
        public int DepartmentId { get; set; }

    }


}