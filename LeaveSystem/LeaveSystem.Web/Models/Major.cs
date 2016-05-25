using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace LeaveSystem.Web.Models
{
    public class Major : Entity<int>
    {
        [Required]
        [Display(Name = "专业名称")]
        public string Name { get; set; }

        public virtual Department Department { get; set; }
    }
}