using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveSystem.Web.ViewModels
{
    public class DepartmentCreateViewModel
    {
        [Required]
        [Display(Name = "学院名称")]
        public string Name { get; set; }
    }
}