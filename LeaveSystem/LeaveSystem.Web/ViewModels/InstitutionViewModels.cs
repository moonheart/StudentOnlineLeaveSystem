using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LeaveSystem.Web.Models;
using Newtonsoft.Json;

namespace LeaveSystem.Web.ViewModels
{
    public class ClassCreateViewModel
    {
        [Required]
        [Display(Name = "年级")]
        public int GradeId { get; set; }
        [Required]
        [Display(Name = "专业")]
        public int MajorId { get; set; }
        [Required]
        [Display(Name = "学院")]
        public int DepartmentId { get; set; }
        [Required]
        [Display(Name = "班别")]
        public string Defination { get; set; }

        [Required]
        [Display(Name = "班主任")]
        public string ClassTeacherId { get; set; }

        public string[] StudentIds { get; set; }

    }
    public class ClassEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "年级")]
        public int GradeId { get; set; }

        [Required]
        [Display(Name = "专业")]
        public int MajorId { get; set; }

        [Required]
        [Display(Name = "学院")]
        public int DepartmentId { get; set; }

        [Required]
        [Display(Name = "班主任")]
        public string ClassTeacherId { get; set; }

        [Required]
        [Display(Name = "班别")]
        public string Defination { get; set; }

        public string[] IdsToRemove { get; set; }

        public ICollection<User> Students { get; set; }

    }

    public class DepartmentEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "院系名称")]
        public string Name { get; set; }

        public int[] IdsToAdd { get; set; }

        public int[] IdsToRemove { get; set; }
    }

    public class MajorCreateViewModel
    {
        [Required]
        [Display(Name = "专业名称")]
        public string MajorName { get; set; }

        [Required]

        public int DepartmentId { get; set; }
    }

    public class MajorEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "专业名称")]
        public string MajorName { get; set; }

        [Required]

        public int DepartmentId { get; set; }
    }

    public class GradeCreateViewModel
    {
        [Required]
        [Display(Name = "年级")]
        public string GradeNumber { get; set; }
    }

    public class GradeEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "年级")]
        public string GradeNumber { get; set; }
    }

    public class AddStudentsViewModel
    {

        [Required]
        public int ClassId { get; set; }

        public string[] StudentsName { get; set; }

        public string[] IdsToAdd { get; set; }
    }

}