using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using LeaveSystem.Web.BLL;
using LeaveSystem.Web.Models;
using LeaveSystem.Web.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using PagedList;

namespace LeaveSystem.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class InstitutionController : Controller
    {

        private InstitutionManager _institutionManager;

        public InstitutionManager InstitutionManager
        {
            get { return _institutionManager ?? HttpContext.GetOwinContext().GetUserManager<InstitutionManager>(); }
            set { _institutionManager = value; }
        }

        private UserManager _userManager;

        public UserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
            set { _userManager = value; }
        }

        public InstitutionController()
        {

        }

        public InstitutionController(InstitutionManager institutionManager)
        {
            _institutionManager = institutionManager;
        }

        // GET: Institution
        public async Task<ActionResult> Index()
        {
            return RedirectToAction("AllClass");
        }



        #region Department

        public async Task<ActionResult> AllDepartment()
        {
            var list = await InstitutionManager.GetAllDepartmentsAsync();
            return View(list);
        }

        public async Task<ActionResult> CreateDepartment()
        {
            ViewBag.Departments = await InstitutionManager.GetAllDepartmentsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(department.Name))
                {
                    var depart = await InstitutionManager.FindDepartmentByNameAsync(department.Name);
                    if (depart == null)
                    {
                        await InstitutionManager.AddDepartmentAsync(department.Name);
                        return RedirectToAction("AllDepartment");
                    }
                    else
                    {
                        ModelState.AddModelError("", "已有相同名称");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "不能为空");
                }
            }
            return View(department);
        }

        public async Task<ActionResult> EditDepartment(int id)
        {
            var de = await InstitutionManager.FindDepartmentByIdAsync(id);
            if (de == null)
                return HttpNotFound();
            ViewBag.InMajors = await InstitutionManager.GetMajorsForDepartmentAsync(de);
            ViewBag.OutMajors = await InstitutionManager.GeNoDepartmentMajorsAsync(de);
            var model = new DepartmentEditViewModel { Id = de.Id, Name = de.Name };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDepartment(DepartmentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var de = await InstitutionManager.FindDepartmentByIdAsync(model.Id);
                if (de != null)
                {
                    if (!string.IsNullOrEmpty(model.Name))
                        de.Name = model.Name;
                    await InstitutionManager.UpdateDepartmentAsync(de);
                    await InstitutionManager.AddMajorsToDepartmentAsync(de, model.IdsToAdd ?? new int[] { });
                    await InstitutionManager.RemoveMajorsFromDepartmentAsync(de, model.IdsToRemove ?? new int[] { });
                    return RedirectToAction("AllDepartment");
                }
                else
                {
                    ModelState.AddModelError("", "学院不存在");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
            if (ModelState.IsValid)
            {
                var major = await InstitutionManager.FindDepartmentByIdAsync(id);
                if (major != null)
                {
                    await InstitutionManager.DeleteDepartmentAsync(major);
                }
            }
            return RedirectToAction("AllDepartment");
        }


        #endregion

        #region Major

        public async Task<ActionResult> AllMajor()
        {
            var list = await InstitutionManager.GetAllMajorsAsync();
            return View(list);
        }

        public async Task<ActionResult> CreateMajor()
        {
            ViewBag.Majors = await InstitutionManager.GetAllMajorsAsync();
            var departments = await InstitutionManager.GetAllDepartmentsAsync();
            SelectList selectList = new SelectList(departments, "Id", "Name");
            ViewBag.selectList = selectList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateMajor(MajorCreateViewModel major)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(major.MajorName))
                {
                    ModelState.AddModelError("", "MajorName不能为空");
                }
                else if (major.DepartmentId == 0)
                {
                    ModelState.AddModelError("", "DepartmentId不能为空");
                }
                else
                {
                    var model = await InstitutionManager.FindMajorByNameAsync(major.MajorName);
                    if (model == null)
                    {
                        await InstitutionManager.AddMajorAsync(major.MajorName);
                        model = await InstitutionManager.FindMajorByNameAsync(major.MajorName);
                        var depart = await InstitutionManager.FindDepartmentByIdAsync(major.DepartmentId);
                        if (depart == null)
                        {
                            ModelState.AddModelError("", "找不到对应学院");
                        }
                        await InstitutionManager.AddMajorsToDepartmentAsync(depart, new[] { model.Id });
                        return RedirectToAction("AllMajor");
                    }
                    else
                    {
                        ModelState.AddModelError("", "已有相同名称");
                    }

                }
            }
            return View(major);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var major = await InstitutionManager.FindMajorByIdAsync(id);
                if (major != null)
                {
                    await InstitutionManager.DeleteMajorAsync(major);
                }
            }
            return RedirectToAction("AllMajor");
        }

        public async Task<ActionResult> EditMajor(int? id)
        {
            var major = await InstitutionManager.FindMajorByIdAsync(id ?? 0);
            if (major == null)
                return HttpNotFound();
            var departments = await InstitutionManager.GetAllDepartmentsAsync();
            SelectList selectList = new SelectList(departments, "Id", "Name");
            ViewBag.selectList = selectList;
            var model = new MajorEditViewModel { Id = major.Id, DepartmentId = major.Department.Id, MajorName = major.Name };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMajor(MajorEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var major = await InstitutionManager.FindMajorByIdAsync(model.Id);
                if (major == null)
                {
                    ModelState.AddModelError("", "未找到此项major");
                    return View(model);
                }
                var depart = await InstitutionManager.FindDepartmentByIdAsync(model.DepartmentId);
                if (depart == null)
                {
                    ModelState.AddModelError("", "未找到此项depart");
                    return View(model);
                }
                major.Name = model.MajorName;
                major.Department = depart;
                await InstitutionManager.UpdateMajorAsync(major);
                return RedirectToAction("AllMajor");
            }
            return View(model);
        }

        #endregion

        #region Grade

        public async Task<ActionResult> AllGrade()
        {
            var list = await InstitutionManager.GetAllGradesAsync();
            return View(list);
        }

        public async Task<ActionResult> CreateGrade()
        {
            ViewBag.Grades = await InstitutionManager.GetAllGradesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateGrade(GradeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.GradeNumber == 0)
                {
                    ModelState.AddModelError("", "GradeNumber不能为0");
                }
                else
                {
                    var grade = await InstitutionManager.FindGradeByGradeNumberAsync(model.GradeNumber);
                    if (grade == null)
                    {
                        await InstitutionManager.AddGradeAsync(model.GradeNumber);
                        return RedirectToAction("AllGrade");
                    }
                    else
                    {
                        ModelState.AddModelError("", "已有相同名称");
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteGrade(int? id)
        {
            if (ModelState.IsValid)
            {
                var model = await InstitutionManager.FindGradeByIdAsync(id ?? 0);
                if (model != null)
                {
                    await InstitutionManager.DeleteGradeAsync(model);
                }
            }
            return RedirectToAction("AllGrade");
        }

        public async Task<ActionResult> EditGrade(int? id)
        {
            var grade = await InstitutionManager.FindGradeByIdAsync(id ?? 0);
            if (grade == null)
                return HttpNotFound();
            var grades = await InstitutionManager.GetAllGradesAsync();
            ViewBag.Grades = grades;
            return View(new GradeEditViewModel { GradeNumber = grade.GradeNum, Id = grade.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditGrade(GradeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var grade = await InstitutionManager.FindGradeByIdAsync(model.Id);
                if (grade == null)
                {
                    ModelState.AddModelError("", "未找到此项grade");
                    return View(model);
                }
                grade.GradeNum = model.GradeNumber;
                await InstitutionManager.UpdateGradeAsync(grade);
                return RedirectToAction("AllGrade");
            }
            return View(model);
        }

        #endregion

        #region Class

        public async Task<ActionResult> AllClass()
        {
            var list = await InstitutionManager.GetAllClassesAsync();
            return View(list);
        }


        public async Task<ActionResult> CreateClass()
        {
            var departments = await InstitutionManager.GetAllDepartmentsAsync();
            SelectList departSelectList = new SelectList(departments, "Id", "Name");
            ViewBag.departSelectList = departSelectList;

            var majors = await InstitutionManager.GetMajorsForDepartmentAsync(departments.ElementAtOrDefault(0));
            SelectList majorSelectList = new SelectList(majors, "Id", "Name");
            ViewBag.majorSelectList = majorSelectList;

            var grades = await InstitutionManager.GetAllGradesAsync();
            SelectList gradeSelectList = new SelectList(grades, "Id", "GradeNum");
            ViewBag.gradeSelectList = gradeSelectList;

            ViewBag.Classes = await InstitutionManager.GetAllClassesAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateClass(ClassCreateViewModel model)
        {
            var departments = await InstitutionManager.GetAllDepartmentsAsync();
            SelectList departSelectList = new SelectList(departments, "Id", "Name");
            ViewBag.departSelectList = departSelectList;

            var majors = await InstitutionManager.GetMajorsForDepartmentAsync(departments.ElementAtOrDefault(0));
            SelectList majorSelectList = new SelectList(majors, "Id", "Name");
            ViewBag.majorSelectList = majorSelectList;

            var grades = await InstitutionManager.GetAllGradesAsync();
            SelectList gradeSelectList = new SelectList(grades, "Id", "GradeNum");
            ViewBag.gradeSelectList = gradeSelectList;

            ViewBag.Classes = await InstitutionManager.GetAllClassesAsync();

            if (ModelState.IsValid)
            {
                var grade = await InstitutionManager.FindGradeByIdAsync(model.GradeId);
                if (grade == null)
                {
                    ModelState.AddModelError("", "找不到grade");
                    return View(model);
                }
                var major = await InstitutionManager.FindMajorByIdAsync(model.MajorId);
                if (major == null)
                {
                    ModelState.AddModelError("", "找不到major");
                    return View(model);
                }
                var class1 = new Class
                {
                    ClassDefination = model.Defination,
                    Grade = grade,
                    Major = major
                };
                if (await InstitutionManager.IsClassExistAsync(c =>
                    !c.IsDeleted && c.Grade.Id == class1.Grade.Id && c.Major.Id == class1.Major.Id &&
                    c.ClassDefination == class1.ClassDefination))
                {
                    ModelState.AddModelError("", "已存在Class");
                    return View(model);
                }
                await InstitutionManager.AddClassAsync(class1);
                return RedirectToAction("AllClass");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> GetMajorOfDepartment(int departId)
        {
            var depart = await InstitutionManager.FindDepartmentByIdAsync(departId);
            if (depart == null)
            {
                return Json(new { error = 1, message = "找不到该Department" });
            }
            var s = Json(new { error = 0, message = "获取成功", data = depart.Majors.Select(m => new { m.Id, m.Name }) });
            return s;
        }

        public async Task<ActionResult> EditClass(int? id)
        {
            var class1 = await InstitutionManager.FindClassByIdAsync(id ?? 0);
            if (class1 == null)
                return HttpNotFound("class不存在");

            var departments = await InstitutionManager.GetAllDepartmentsAsync();
            SelectList departSelectList = new SelectList(departments.ToList(), "Id", "Name");
            ViewBag.departSelectList = departSelectList;

            var majors = await InstitutionManager.GetMajorsForDepartmentAsync(departments.ElementAtOrDefault(0));
            SelectList majorSelectList = new SelectList(majors.ToList(), "Id", "Name");
            ViewBag.majorSelectList = majorSelectList;

            var grades = await InstitutionManager.GetAllGradesAsync();
            SelectList gradeSelectList = new SelectList(grades.ToList(), "Id", "GradeNum");
            ViewBag.gradeSelectList = gradeSelectList;

            ViewBag.Classes = await InstitutionManager.GetAllClassesAsync();

            var model = new ClassEditViewModel
            {
                Defination = class1.ClassDefination,
                DepartmentId = class1.Major.Department.Id,
                GradeId = class1.Grade.Id,
                MajorId = class1.Major.Id,
                Students = class1.Students
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditClass(ClassEditViewModel model)
        {
            var departments = await InstitutionManager.GetAllDepartmentsAsync();
            SelectList departSelectList = new SelectList(departments.ToList(), "Id", "Name");
            ViewBag.departSelectList = departSelectList;

            var majors = await InstitutionManager.GetMajorsForDepartmentAsync(departments.ElementAtOrDefault(0));
            SelectList majorSelectList = new SelectList(majors.ToList(), "Id", "Name");
            ViewBag.majorSelectList = majorSelectList;

            var grades = await InstitutionManager.GetAllGradesAsync();
            SelectList gradeSelectList = new SelectList(grades.ToList(), "Id", "GradeNum");
            ViewBag.gradeSelectList = gradeSelectList;

            ViewBag.Classes = await InstitutionManager.GetAllClassesAsync();

            if (ModelState.IsValid)
            {
                var class1 = await InstitutionManager.FindClassByIdAsync(model.Id);
                if (class1 == null)
                {
                    ModelState.AddModelError("", "找不到class1");
                    return View(model);
                }
                var grade = await InstitutionManager.FindGradeByIdAsync(model.GradeId);
                if (grade == null)
                {
                    ModelState.AddModelError("", "找不到grade");
                    return View(model);
                }
                var major = await InstitutionManager.FindMajorByIdAsync(model.MajorId);
                if (major == null)
                {
                    ModelState.AddModelError("", "找不到major");
                    return View(model);
                }
                class1.Major = major;
                class1.Grade = grade;
                class1.ClassDefination = model.Defination;

                await InstitutionManager.UpdateClassAsync(class1);

                await UserManager.RemoveStudentsFromClassAsync(model.IdsToRemove ?? new string[] { });

                return RedirectToAction("AllClass");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteClass(int? id)
        {
            if (ModelState.IsValid)
            {
                var model = await InstitutionManager.FindClassByIdAsync(id ?? 0);
                if (model != null)
                {
                    await InstitutionManager.DeleteClassAsync(model);
                }
            }
            return RedirectToAction("AllClass");
        }

        public async Task<ActionResult> AddStudent(int? id, int? p)
        {
            var class1 = await InstitutionManager.FindClassByIdAsync(id ?? 0);
            if (class1 == null)
            {
                ModelState.AddModelError("id", "The Class you are looking for is not Exist");
                return View("Error", new[] { "The Class you are looking for is not Exist" });
                //return HttpNotFound("The Class you are looking for is not Exist");
            }
            var students = UserManager.GetNoClassStudents().ToList().ToPagedList(p ?? 1, 10);
            ViewBag.OutStudents = students;
            ViewBag.Class = class1;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddStudent(AddStudentsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var class1 = await InstitutionManager.FindClassByIdAsync(model.ClassId);
                if (class1 == null)
                {
                    return View("Error", new[] { "班级找不到" });
                }
                await UserManager.AddStudentsToClassAsync(class1, model.IdsToAdd ?? new string[] { });
                model.IdsToAdd = new string[] { };
                return RedirectToAction("AddStudent");

            }
            return View(model);

        }

        #endregion

    }
}